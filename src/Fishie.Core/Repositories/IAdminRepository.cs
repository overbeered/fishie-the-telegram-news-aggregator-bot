using Fishie.Core.Models;

namespace Fishie.Core.Repositories;

/// <summary>
///  Defines Admin repository interface
/// </summary>
public interface IAdminRepository
{
    /// <summary>
    /// Adds an administrator
    /// </summary>
    /// <param name="admin">Telegram administrator model for chat</param>
    Task AddAsync(Admin admin);

    /// <summary>
    /// Finds the administrator by username
    /// </summary>
    /// <param name="username">Username</param>
    /// <returns>Telegram admin model for chat</returns>
    Task<Admin?> FindAsync(string username);

    /// <summary>
    /// Removes the administrator by username
    /// </summary>
    /// <param name="username">Username</param>
    Task RemoveAsync(string username);

    /// <summary>
    /// Checks whether the administrator exists by the identifier
    /// </summary>
    /// <param name="id">Identifier</param>
    /// <returns>True - exists, false - does not exist</returns>
    Task<bool> ExistsAsync(long id);

    /// <summary>
    /// Returns a list of all administrators
    /// </summary>
    /// <returns>List of administrators</returns>
    Task<List<Admin?>> FindAllAsync();
}