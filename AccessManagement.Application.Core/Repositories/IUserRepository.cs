using AccessManagement.Domain.Entities;

namespace AccessManagement.Application.Core.Repositories;

public interface IUserRepository
{
  Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

  Task<User> InsertAsync(User user, CancellationToken cancellationToken = default);
}
