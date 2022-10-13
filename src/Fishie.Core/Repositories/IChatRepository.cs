using Fishie.Core.Models;

namespace Fishie.Core.Repositories;

/// <summary>
/// Defines chat repository interface
/// </summary>
public interface IChatRepository
{
    /// <summary>
    /// Adds an chat
    /// </summary>
    /// <param name="chat">Telegram chat model</param>
    /// <returns></returns>
    Task AddAsync(Chat chat);

    /// <summary>
    /// Returns a list of all chat
    /// </summary>
    /// <returns>List of chat</returns>
    Task<List<Chat?>> FindAllAsync();

    /// <summary>
    /// Finds the chat by name
    /// </summary>
    /// <param name="chatName">Chat name</param>
    /// <returns>Telegram chat model</returns>
    Task<Chat?> FindAsync(string chatName);

    /// <summary>
    /// Finds the chat by identifier
    /// </summary>
    /// <param name="id">Identifier</param>
    /// <returns>Telegram chat model</returns>
    Task<Chat?> FindAsync(long id);

    /// <summary>
    /// Removes the chat by name
    /// </summary>
    /// <param name="chatName">Chat name</param>
    Task RemoveAsync(string chatName);

    /// <summary>
    /// Removes the chat by identifier
    /// </summary>
    /// <param name="id">Identifier</param>
    Task RemoveAsync(long id);

    /// <summary>
    /// Checks whether the chat exists
    /// </summary>
    /// <param name="chat">Telegram chat model</param>
    /// <returns>True - exists, false - does not exist</returns>
    Task<bool> ExistsAsync(Chat chat);
}