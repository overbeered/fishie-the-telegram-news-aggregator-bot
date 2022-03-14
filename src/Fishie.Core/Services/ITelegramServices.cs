using Fishie.Core.Models;
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
    }
}
