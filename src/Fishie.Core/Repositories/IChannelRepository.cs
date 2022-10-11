using Fishie.Core.Models;

namespace Fishie.Core.Repositories;

/// <summary>
/// Defines Channel repository interface.
/// </summary>
public interface IChannelRepository
{

    Task AddAsync(Channel channel);

    Task<List<Channel?>> FindAllAsync();

    Task<Channel?> FindAsync(string username);

    Task<Channel?> FindAsync(long id);

    Task DeleteAsync(string username);

    Task DeleteAsync(long id);

    Task<bool> ExistsAsync(Channel channel);
}