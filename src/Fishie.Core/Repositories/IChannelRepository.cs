using Fishie.Core.Models;

namespace Fishie.Core.Repositories;

/// <summary>
/// Defines channel repository interface.
/// </summary>
public interface IChannelRepository
{
    /// <summary>
    /// Adds an channel
    /// </summary>
    /// <param name="channel">Telegram channel model</param>
    Task AddAsync(Channel channel);

    /// <summary>
    /// Returns a list of all channel
    /// </summary>
    /// <returns>List of channel</returns>
    Task<List<Channel?>> FindAllAsync();

    /// <summary>
    /// Finds the channel by username
    /// </summary>
    /// <param name="username">Username</param>
    /// <returns>Telegram channel model</returns>
    Task<Channel?> FindAsync(string username);

    /// <summary>
    /// Finds the channel by identifier
    /// </summary>
    /// <param name="id">Identifier</param>
    /// <returns>Telegram channel model</returns>
    Task<Channel?> FindAsync(long id);

    /// <summary>
    /// Removes the channel by username
    /// </summary>
    /// <param name="username">Username</param>
    Task RemoveAsync(string username);

    /// <summary>
    /// Removes the channel by identifier
    /// </summary>
    /// <param name="id">Identifier</param>
    Task RemoveAsync(long id);

    /// <summary>
    /// Checks whether the channel exists
    /// </summary>
    /// <param name="channel">Telegram channel model</param>
    /// <returns>True - exists, false - does not exist</returns>
    Task<bool> ExistsAsync(Channel channel);
}