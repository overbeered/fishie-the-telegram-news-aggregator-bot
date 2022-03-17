using Fishie.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fishie.Core.Services
{
    /// <summary>
    /// Service for working with channels, chats
    /// </summary>
    public interface IChannelServices
    {
        /// <summary>
        /// Adds a channel\chat
        /// </summary>
        /// <param name="channelName">Channel\chat name</param>
        /// <returns></returns>
        Task AddChannelAsync(string channelName);

        /// <summary>
        /// Returns channels
        /// </summary>
        /// <returns>Channels model</returns>
        Task<IEnumerable<Channel?>?> GetAllChannelsAsync();

        /// <summary>
        /// Returns the channel\chat by name
        /// </summary>
        /// <param name="channelName">Channel name</param>
        /// <returns>Channel model</returns>
        Task<Channel?> GetChannelAsync(string channelName);

        /// <summary>
        /// Returns the channel\chat by id
        /// </summary>
        /// <param name="id">Сhannel\chat id</param>
        /// <returns>Channel model</returns>
        Task<Channel?> GetChannelByIdAsync(long id);

        /// <summary>
        /// Deletes a channel\chat by name
        /// </summary>
        /// <param name="channelName">Сhannel\chat name</param>
        /// <returns>true - channel deleted, false - channel has not been deleted</returns>
        Task<bool> DeleteChannelAsync(string channelName);

        /// <summary>
        /// Deletes a channel\chat by id
        /// </summary>
        /// <param name="id">Сhannel\chat id</param>
        /// <returns>true - channel deleted, false - channel has not been deleted</returns>
        Task<bool> DeleteChannelByIdAsync(long id);

        /// <summary>
        /// Subscribes to the channel in telegram
        /// </summary>
        /// <param name="channelName">Сhannel name</param>
        /// <returns></returns>
        Task SubscribeAsync(string channelName);

        /// <summary>
        /// Unsubscribes to the channel in the telegram
        /// </summary>
        /// <param name="channelName">Сhannel name</param>
        /// <returns></returns>
        Task UnsubscribeAsync(string channelName);

        /// <summary>
        /// Gets the history of messages in the chat\channel
        /// </summary>
        /// <param name="channelName">Сhannel\chat name</param>
        /// <param name="count">The number of messages starting from newer ones. By default, the value is 5</param>
        /// <returns>List of messages</returns>
        Task<List<string?>?> GetMessagesChannelAsync(string channelName, int count = 5);

        /// <summary>
        /// Sends a message to the channel\chat
        /// </summary>
        /// <param name="channelName">Сhannel\chat name</param>
        /// <param name="message">Message to send</param>
        /// <returns></returns>
        Task SendMessagesChannelAsync(string channelName, string message);
    }
}
