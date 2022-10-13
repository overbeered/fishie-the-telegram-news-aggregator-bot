using Fishie.Core.Models;

namespace Fishie.Core.Repositories;

/// <summary>
/// Defines the interface for the storage of forwarded messages
/// </summary>
public interface IForwardMessagesRepository
{
    /// <summary>
    /// Adds an model for forwarding messages 
    /// </summary>
    /// <param name="forwardMessages">Model for forwarding messages to a chat</param>
    Task AddAsync(ForwardMessages forwardMessages);

    /// <summary>
    /// Removes the model for forwarding messages
    /// </summary>
    /// <param name="forwardMessages">Model for forwarding messages to a chat</param>
    Task RemoveAsync(ForwardMessages forwardMessages);

    /// <summary>
    /// Returns a list of all models for forwarding messages
    /// </summary>
    /// <returns>List models for forwarding messages</returns>
    Task<List<ForwardMessages?>> FindAllAsync();

    /// <summary>
    /// Finds a list of models for forwarding messages by channel identifier
    /// </summary>
    /// <param name="channelId">Channel identifier</param>
    /// <returns>List models for forwarding messages</returns>
    Task<List<ForwardMessages?>> FindChannelIdAsync(long channelId);

    /// <summary>
    /// Finds a list of models for forwarding messages by chat identifier
    /// </summary>
    /// <param name="chatId">Chat identifier</param>
    /// <returns>List models for forwarding messages</returns>
    Task<List<ForwardMessages?>> FindChatIdAsync(long chatId);

    /// <summary>
    /// Removes the model for forwarding messages by channel identifier
    /// </summary>
    /// <param name="channnelId">Channel identifier</param>
    Task RemoveChannelIdAsync(long channnelId);

    /// <summary>
    /// Removes the model for forwarding messages by chat identifier
    /// </summary>
    /// <param name="chatId">Chat identifier</param>
    Task RemoveChatIdAsync(long chatId);

    /// <summary>
    /// Checks whether the model for forwarding messages exists
    /// </summary>
    /// <param name="forwardMessages">Model for forwarding messages to a chat</param>
    /// <returns>True - exists, false - does not exist</returns>
    Task<bool> ExistsAsync(ForwardMessages forwardMessages);

    /// <summary>
    /// Checks whether the model for forwarding messages exists by chat identifier
    /// </summary>
    /// <param name="chatId">Chat identifier</param>
    /// <returns>True - exists, false - does not exist</returns>
    Task<bool> ChatIdExistsAsync(long chatId);

    /// <summary>
    /// Checks whether the model for forwarding messages exists by channel identifier
    /// </summary>
    /// <param name="channelId">Channel identifier</param>
    /// <returns>True - exists, false - does not exist</returns>
    Task<bool> ChannelIdExistsAsync(long channelId);
}