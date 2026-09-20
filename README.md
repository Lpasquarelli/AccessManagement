# Access Management API

API .NET 10 organizada em Clean Architecture, com SQL Server, EF Core, Redis, Swagger e versionamento por URL.

## Dependências locais

Copie `.env.example` para `.env`, defina uma senha local forte e inicie SQL Server e Redis:

```bash
cp .env.example .env
docker compose up -d
```

Configure a mesma senha na connection string usando user-secrets:

```bash
dotnet user-secrets --project AccessManagement.API set \
  "Database:ConnectionString" \
  "Server=localhost,1433;Database=AccessManagement;User Id=sa;Password=<senha>;TrustServerCertificate=True;Encrypt=True"
```

Execute a API:

```bash
dotnet run --project AccessManagement.API
```

Em `Development`, a migration é aplicada automaticamente e o Swagger fica disponível em `/swagger`.

## Endpoints iniciais

- `POST /api/v1/users`
- `GET /api/v1/users/{id}`

O Redis não recebe comandos de escrita pendentes. Ele mantém cache fresco e um snapshot de fallback para consultas por ID quando o SQL Server estiver temporariamente indisponível.
