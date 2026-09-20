using AccessManagement.Application.Core.Exceptions;
using AccessManagement.Application.Core.Repositories;
using AccessManagement.Domain.Entities;
using AccessManagement.Infrastructure.Context;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AccessManagement.Infrastructure.Repositories;

public sealed class UserRepository(
  AccessManagementDbContext dbContext,
  ILogger<UserRepository> logger) : IUserRepository
{
  public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
  {
    logger.LogDebug(
      "[UserRepository][GetByIdAsync] Querying user in SQL Server.");

    try
    {
      return await dbContext.Users
        .AsNoTracking()
        .SingleOrDefaultAsync(user => user.Id == id && user.Active, cancellationToken);
    }
    catch (SqlException exception)
    {
      logger.LogError(
        exception,
        "[UserRepository][GetByIdAsync] SQL Server communication failed while querying a user.");

      throw new DataStoreUnavailableException("SQL Server is unavailable.", exception);
    }
  }

  public async Task<User> InsertAsync(User user, CancellationToken cancellationToken = default)
  {
    logger.LogDebug(
      "[UserRepository][InsertAsync] Inserting user in SQL Server.");

    try
    {
      await dbContext.Users.AddAsync(user, cancellationToken);
      await dbContext.SaveChangesAsync(cancellationToken);
      return user;
    }
    catch (DbUpdateException exception) when (exception.InnerException is SqlException)
    {
      logger.LogError(
        exception,
        "[UserRepository][InsertAsync] SQL Server communication failed while inserting a user.");

      throw new DataStoreUnavailableException("SQL Server is unavailable.", exception);
    }
    catch (SqlException exception)
    {
      logger.LogError(
        exception,
        "[UserRepository][InsertAsync] SQL Server communication failed while inserting a user.");

      throw new DataStoreUnavailableException("SQL Server is unavailable.", exception);
    }
  }

  public async Task<User> UpdateAsync(User user, CancellationToken cancellationToken = default)
  {
    logger.LogDebug(
      "[UserRepository][UpdateAsync] Updating user in SQL Server.");

    try
    {
      dbContext.Users.Update(user);
      await dbContext.SaveChangesAsync(cancellationToken);
      return user;
    }
    catch (DbUpdateException exception) when (exception.InnerException is SqlException)
    {
      logger.LogError(
        exception,
        "[UserRepository][UpdateAsync] SQL Server communication failed while updating a user.");

      throw new DataStoreUnavailableException("SQL Server is unavailable.", exception);
    }
    catch (SqlException exception)
    {
      logger.LogError(
        exception,
        "[UserRepository][UpdateAsync] SQL Server communication failed while updating a user.");

      throw new DataStoreUnavailableException("SQL Server is unavailable.", exception);
    }
  }
}
