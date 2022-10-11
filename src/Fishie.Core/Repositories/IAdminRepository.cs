using Fishie.Core.Models;

namespace Fishie.Core.Repositories;

/// <summary>
///  Defines Admin repository interface.
/// </summary>
public interface IAdminRepository
{
    Task AddAsync(Admin admin);

    Task<Admin?> FindAsync(string username);

    Task DeleteUsernameAsync(string username);

    Task<bool> ExistsAsync(long id);

    Task<List<Admin?>> FindAllAsync();
}