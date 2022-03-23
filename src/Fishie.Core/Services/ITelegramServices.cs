using Fishie.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fishie.Core.Services
{
    /// <summary>
    /// Service for working with telegram
    /// </summary>
    public interface ITelegramServices
    {
        /// <summary>
        /// Login as a user (if not already logged-in).
        /// </summary>
        /// <returns></returns>
        Task LoginAsync();

        /// <summary>
        /// Disconnect from Telegram 
        /// </summary>
        void Reset();

        /// <summary>
        /// Has this Client established connection been disconnected?
        /// </summary>
        bool Disconnected { get; }

        ///// <summary>
        ///// Channel\chat search by name in telegram
        ///// </summary>
        ///// <param name="query">Сhannel name</param>
        ///// <returns>Сhannel model</returns>
        //Task<Channel?> SearchChannelAsync(string query);

        ///// <summary>
        ///// Subscribes to the channel in telegram
        ///// </summary>
        ///// <param name="channel">Сhannel model</param>
        ///// <returns></returns>
        //Task SubscribeAsync(Channel channel);

        ///// <summary>
        ///// Unsubscribes to the channel in the telegram
        ///// </summary>
        ///// <param name="channel">Сhannel model</param>
        ///// <returns></returns>
        //Task UnsubscribeAsync(Channel channel);

        ///// <summary>
        ///// Gets the history of messages in the chat\channel
        ///// </summary>
        ///// <param name="channel">Сhannel model</param>
        ///// <param name="count">The number of messages starting from newer ones. By default, the value is 5</param>
        ///// <returns>List of messages</returns>
        //Task<List<string?>?> GetMessagesChannelAsync(Channel channel, int count = 5);

        ///// <summary>
        ///// Sends a message to the channel\chat
        ///// </summary>
        ///// <param name="channel">Сhannel model</param>
        ///// <param name="message">Message to send</param>
        ///// <returns></returns>
        //Task SendMessagesChannelAsync(Channel channel, string message);
    }
}
