# Access Management

API de configuração e consulta de acessos, perfis, permissões e alçadas para Internet Banking.

Este serviço começa depois da autenticação e termina antes do workflow transacional. Ele não valida tokens, não cria transferências e não registra decisões de aprovação. O serviço de login fornece o `AuthenticationId`; os serviços consumidores consultam esta API para descobrir contas, permissões e regras efetivas.

O projeto utiliza .NET 10, ASP.NET Core, Entity Framework Core, SQL Server e Redis. A organização segue Clean Architecture e separa domínio, contratos, casos de uso, infraestrutura e transporte HTTP.

## Estrutura da solution

- `AccessManagement.Domain`: entidades anêmicas e factories de domínio.
- `AccessManagement.Application.Core`: contratos de repositórios e modelos de consulta.
- `AccessManagement.Application`: Use Cases, Inputs, Outputs e Results.
- `AccessManagement.Infrastructure`: EF Core, SQL Server, Redis, repositórios e migrations.
- `AccessManagement.API`: controllers, versionamento, Swagger e conversão de Results para HTTP.

As dependências apontam para dentro da arquitetura. O domínio não depende de EF Core ou ASP.NET Core.

## Modelo de autorização

| Tabela | Responsabilidade |
| --- | --- |
| `Context` | Modalidade operacional e moeda, por exemplo `ONSHORE` e `OFFSHORE`. |
| `User` | Usuário do Internet Banking e referência ao serviço de autenticação. |
| `Account` | Conta bancária pertencente a um contexto operacional. |
| `UserAccount` | Relacionamento entre usuário e conta, incluindo titularidade e usuário mestre. |
| `Profile` | Perfil de acesso exclusivo de uma conta. |
| `Permission` | Catálogo de ações autorizáveis. |
| `PermissionGroup` | Organização visual de permissões por contexto. |
| `ProfilePermission` | Permissões concedidas a um perfil. |
| `UserAccountProfiles` | Perfis atribuídos ao acesso de um usuário em uma conta. |
| `Authority` | Tipo de operação sujeita a alçada. |
| `AuthorityApprovalRole` | Faixa de valor e quantidade mínima de aprovadores. |
| `ApprovalRoleProfile` | Perfis autorizados a participar de uma alçada. |

Não existem exclusões em cascata. Relacionamentos e configurações que possuem `Active` devem ser desativados logicamente para preservar rastreabilidade.

## Convenções do banco

- Chaves primárias: `UNIQUEIDENTIFIER` com `NEWSEQUENTIALID()`.
- Datas: `DATETIME2(7)` em UTC com `SYSUTCDATETIME()`.
- Valores monetários: `DECIMAL(19,4)`.
- `Active = 1`: registro ativo.
- `Active = 0`: registro desativado.
- `CreatedBy` e `UpdatedBy`: `NVARCHAR(150)`, pois são identificadores externos ao serviço.
- FKs usam `NO ACTION`/`Restrict`.
- Constraints, checks e índices mantêm os nomes definidos no modelo SQL.

A alçada exige `MinApprovers > 0`. Uma faixa ilimitada deve ter `ValueLimit = NULL`; uma faixa limitada deve ter `ValueLimit > 0`. Índices filtrados impedem faixas ativas ambíguas para a mesma autoridade.

## Fluxo de autorização

1. O usuário é relacionado à conta por `UserAccount`.
2. Usuários não mestres recebem um ou mais perfis por `UserAccountProfiles`.
3. Cada perfil recebe permissões por `ProfilePermission`.
4. Perfis aprovadores são relacionados às faixas por `ApprovalRoleProfile`.
5. O módulo transacional consulta esta API para decidir quem pode operar ou aprovar.

Transferências, solicitações de aprovação e decisões dos aprovadores não pertencem a este projeto.

Uma regra somente é retornada quando toda a cadeia aplicável está ativa: `User`, `Account`, `UserAccount`, `UserAccountProfiles` e `Profile`. Uma inconsistência ou falha no SQL Server nunca concede acesso.

`User` sempre representa a pessoa. O termo operador não é usado como outro domínio: um usuário que opera uma conta é representado pelo relacionamento `UserAccount`.

`Account.Identifier` é a referência funcional única da conta e deve ser usado nas rotas, comandos e consultas da API. `Account.Id` permanece apenas como chave técnica interna para as relações do banco de dados e não é exposto como referência de conta.

Titulares e usuários mestres são considerados dados preexistentes neste escopo. Um usuário mestre recebe todas as permissões disponíveis no contexto da conta sem precisar de perfil. A API não cria titulares neste momento; o fluxo de criação disponível é exclusivo para operadores, sempre com `IsMaster = false` e `IsHolder = false`.

## Padrão de Use Case

Cada funcionalidade deve possuir sua própria pasta:

```text
UseCases/<Dominio>/<Funcionalidade>/
  <Funcionalidade>UseCaseInput.cs
  <Funcionalidade>UseCaseOutput.cs
  I<Funcionalidade>UseCase.cs
  <Funcionalidade>UseCase.cs
```

Todo Input implementa:

```csharp
(bool IsValid, string[] Errors) ValidateInput();
```

Todo Use Case implementa `IUseCase<TInput, TOutput>`, cuja única operação é `HandleAsync`. A API devolve `Result.Data` diretamente em sucesso: arrays não recebem propriedades como `users`, `profiles` ou `approvalRoles`, e objetos não recebem um envelope adicional. Em falha, retorna `{ message, errors? }`.

## Usuários

O CRUD de usuário está disponível em:

```text
GET    /api/v1/users/{id}
PUT    /api/v1/users
DELETE /api/v1/users/{id}?updatedBy=<identificador>
```

Operadores são criados pelo endpoint da conta documentado abaixo. O DELETE é lógico e define `Active = false`. Usuários inativos não são retornados pelo GET e não podem ser atualizados ou excluídos novamente.

## Cache Redis

A consulta de usuário por ID segue esta ordem:

1. cache fresco;
2. SQL Server quando não existe cache fresco;
3. cache de fallback apenas quando o SQL Server está indisponível.

PUT e DELETE invalidam o cache fresco e o fallback. O TTL pode ser informado em minutos ao gravar ou obtido de `Redis:FreshTtlMinutes` e `Redis:FallbackTtlMinutes`.

O catálogo de contextos e permissões também usa Redis, pois é uma consulta frequente e de baixa volatilidade. As consultas de acesso efetivo e avaliação de alçada leem o SQL Server diretamente para que uma revogação tenha efeito imediato.

## Catálogo inicial

A migration `SeedAccessCatalog` cria IDs estáveis para:

- contexto `ONSHORE`, moeda `BRL`;
- contexto `OFFSHORE`, moeda `USD`;
- 27 permissões globais;
- os grupos de consulta, aprovação, inclusão e outras permissões do contexto Onshore;
- os grupos de aprovação e inclusão de pagamentos do contexto Offshore.

Os GUIDs funcionais ficam em `PermissionCatalog` e `AccessContextCatalog`. O código nunca usa a descrição mutável como chave de autorização.

A migration `MakeUserAuthenticationIdUnique` torna o `AuthenticationId` único. Esse identificador representa a identidade externa do usuário e não pode apontar para mais de um cadastro.

O catálogo pode ser consultado em:

```text
GET /v1/access-catalog
```

Cada grupo de permissões retorna `hasApprovalPrivileges`. A flag é `true` quando o grupo contém ao menos uma permissão de aprovação e pode ser usado para montar perfis aprovadores. Ela é calculada pelos identificadores estáveis das permissões, não pela descrição do grupo.

## Gestão de perfis, usuários da conta e alçadas

```text
GET    /v1/accounts/{accountIdentifier}/profiles
POST   /v1/accounts/{accountIdentifier}/profiles
PUT    /v1/accounts/{accountIdentifier}/profiles/{profileId}
PATCH  /v1/accounts/{accountIdentifier}/profiles/{profileId}/activation
PUT    /v1/accounts/{accountIdentifier}/profiles/{profileId}/permissions

GET    /v1/accounts/{accountIdentifier}/users?page=1&pageSize=20
POST   /v1/accounts/{accountIdentifier}/users
PATCH  /v1/accounts/{accountIdentifier}/users/{userAccountId}/activation
PUT    /v1/accounts/{accountIdentifier}/users/{userAccountId}/profiles

GET    /v1/accounts/{accountIdentifier}/authorities/{authorityId}/approval-roles
POST   /v1/accounts/{accountIdentifier}/authorities/{authorityId}/approval-roles
PUT    /v1/accounts/{accountIdentifier}/authorities/{authorityId}/approval-roles/{roleId}
PATCH  /v1/accounts/{accountIdentifier}/authorities/{authorityId}/approval-roles/{roleId}/activation
```

O `POST /accounts/{accountIdentifier}/users` cria o operador e o relacionamento com a conta da rota na mesma transação. O relacionamento sempre nasce com `IsMaster = false` e `IsHolder = false`. Os perfis são opcionais na criação e podem ser informados no mesmo payload ou adicionados posteriormente pelo endpoint de substituição de perfis. Todo perfil informado deve estar ativo e pertencer à conta da rota.

O GET de usuários aceita `page` e `pageSize` (de 1 a 100). A resposta contém somente os usuários da página solicitada, sem metadados de paginação. Cada perfil atribuído retorna apenas `name` e `description`; o identificador interno do perfil não é exposto nessa consulta.

Exemplo sem perfis:

```json
{
  "name": "Rebecca",
  "email": "rebecca@example.com",
  "phone": "+5511999999999",
  "taxId": "12345678900",
  "authenticationId": "rebecca@example.com",
  "isBrazilResident": true,
  "createdBy": "account-master"
}
```

Para criar o operador com perfis, inclua `profileIds` com os identificadores desejados. Enviar uma lista vazia ao endpoint de perfis remove todas as atribuições do operador sem desativar o usuário ou seu relacionamento com a conta.

A mesma regra de isolamento vale para os perfis vinculados a uma alçada. Desativar a última alçada de um perfil exige `ConfirmPrivilegeElevation = true`, pois essa alteração pode tornar o perfil ilimitado.

## Consultas para outros serviços

Contas, perfis e permissões efetivas pelo identificador usado no serviço de login:

```text
GET /v1/accesses/users/{authenticationId}
```

Avaliação das regras de um usuário em uma conta e autoridade:

```text
GET /v1/accesses/users/{authenticationId}/accounts/{accountIdentifier}/evaluation?permissionId={permissionId}&authorityId={authorityId}&amount={valor-opcional}
```

A avaliação retorna se a permissão solicitada foi concedida, se o usuário pode criar uma solicitação de aprovação, titularidade, indicador de mestre, perfis ativos, permissões efetivas e todas as alçadas da conta para a autoridade. Ela também identifica em quais alçadas o usuário pode aprovar e, quando `amount` é informado, seleciona a menor faixa que atende ao valor. Um indicador separado informa se o usuário é elegível para a faixa selecionada. Também é informado quando o usuário possui perfil aprovador sem alçada ativa para a autoridade e, portanto, é ilimitado segundo a regra atual. Titulares não criam solicitação de aprovação: o consumidor recebe `IsHolder = true` e segue seu fluxo próprio.

Esses endpoints não autenticam o chamador nesta etapa do projeto. Guard-rails de rede, autenticação entre serviços e autorização do consumidor devem ser adicionados antes da exposição fora do ambiente controlado.

### Campos importantes da avaliação

- `canApprove` indica que o usuário possui permissão de aprovação e está associado a um perfil elegível para alguma alçada ativa.
- `canApproveAlone` indica que, para o `amount` informado, a política selecionada exige apenas um aprovador e o usuário é elegível para essa política.
- `selectedPolicy` é a menor política ativa cujo limite atende ao valor consultado. Se o valor ultrapassar uma faixa, a próxima faixa aplicável é usada.
- `isUnlimitedApprover` identifica um perfil aprovador sem alçada ativa para a autoridade, conforme a regra atual do domínio.

Exemplo reduzido de resposta para uma operação de `90` quando a primeira política possui limite `100` e `MinApprovers = 1`:

```json
{
  "accountIdentifier": "0001",
  "canApprove": true,
  "canApproveAlone": true,
  "selectedPolicy": {
    "valueLimit": 100,
    "minApprovers": 1
  }
}
```

O parâmetro `authorityId` referencia o tipo de operação sujeito a alçada, como a autoridade de pagamentos ou PIX. O campo `actorId`, aceito na criação e atualização de alçadas, identifica o autor externo da alteração para auditoria; ele não altera a identidade resolvida pelo serviço de autenticação.

## Swagger e documentação HTTP

A documentação interativa está disponível em:

```text
GET /swagger
GET /swagger/v1/swagger.json
```

Os endpoints usam as anotações nativas `EndpointSummary` e `EndpointDescription`, que aparecem diretamente no Swagger sem comentários XML. A especificação documenta:

- versionamento por segmento (`/v1/...`);
- `AccountIdentifier` como referência funcional única da conta;
- payloads de criação e atualização de usuários, perfis e alçadas;
- paginação da listagem de usuários por `page` e `pageSize`, sem metadados no retorno;
- respostas diretas, sem envelopes como `users`, `profiles` ou `approvalRoles`;
- o campo `hasApprovalPrivileges` em cada grupo do catálogo;
- a diferença entre `canApprove` e `canApproveAlone` na avaliação de acesso.

Respostas de sucesso retornam diretamente o objeto ou a lista produzida pelo caso de uso. Respostas de erro seguem o contrato `{ message, errors? }`. Os códigos esperados são `400` para entrada inválida, `404` para recurso inexistente ou pertencente a outra conta, `409` para conflito, `422` para configuração de alçada inválida, `503` para indisponibilidade do SQL Server e `500` para erro inesperado.

## Configuração local

Configure SQL Server e Redis em `AccessManagement.API/appsettings.Development.json` ou por variáveis de ambiente. Para não versionar credenciais, prefira:

```bash
export Database__ConnectionString='Server=localhost,1433;Database=AccessManagement;User Id=sa;Password=<senha>;TrustServerCertificate=True;Encrypt=True'
export Redis__ConnectionString='localhost:6379,abortConnect=false'
```

Restaure dependências e ferramentas:

```bash
dotnet restore
dotnet tool restore
```

Execute a API:

```bash
dotnet run --project AccessManagement.API
```

Em `Development`, as migrations são aplicadas automaticamente quando `Database:ApplyMigrationsOnStartup` estiver habilitado. O Swagger fica disponível em `/swagger`.

## Migrations

Criar uma migration:

```bash
dotnet tool run dotnet-ef migrations add NomeDaMigration \
  --project AccessManagement.Infrastructure \
  --startup-project AccessManagement.API \
  --output-dir Context/Migrations
```

Aplicar manualmente:

```bash
dotnet tool run dotnet-ef database update \
  --project AccessManagement.Infrastructure \
  --startup-project AccessManagement.API
```

Gerar o script SQL para revisão:

```bash
dotnet tool run dotnet-ef migrations script \
  --project AccessManagement.Infrastructure \
  --startup-project AccessManagement.API \
  --idempotent
```

## Logs e erros

Os logs seguem o formato:

```text
[NomeDaClasse][NomeDoMetodo] descrição do evento.
```

Dados pessoais, connection strings, valores armazenados no Redis e detalhes internos de exceções não devem ser enviados nas respostas HTTP.
