using Fishie.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fishie.Core.Services
{
    /// <summary>
    /// 
    /// </summary>
    public interface ITelegramServices
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<Channel?> SearchChannelAsync(string query);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="channel"></param>
        /// <returns></returns>
        Task SubscribeAsync(Channel channel);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="channel"></param>
        /// <returns></returns>
        Task UnsubscribeAsync(Channel channel);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        Task<List<string?>?> GetMessagesChannelAsync(Channel channel, int count = 5);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        Task SendMessagesChannelAsync(Channel channel, string message);
    }
}
