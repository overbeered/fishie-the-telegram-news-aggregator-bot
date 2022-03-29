using Fishie.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fishie.Core.Repositories
{
    /// <summary>
    ///  Defines ForwardMessages repository interface.
    /// </summary>
    public interface IForwardMessagesRepository
    {
        /// <summary>
        /// Adds a model ForwardMessages
        /// </summary>
        /// <param name="forwardMessages">Model ForwardMessages</param>
        /// <returns></returns>
        Task AddForwardMessagesAsync(ForwardMessages forwardMessages);

        /// <summary>
        /// Deletes a model ForwardMessages
        /// </summary>
        /// <param name="forwardMessages">Model ForwardMessages</param>
        /// <returns></returns>
        Task DeleteForwardMessagesAsync(ForwardMessages forwardMessages);

        /// <summary>
        /// Returns models ForwardMessages
        /// </summary>
        /// <returns>Model ForwardMessages</returns>
        Task<IEnumerable<ForwardMessages?>?> GetAllForwardMessagesAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="channnelId"></param>
        /// <returns></returns>
        Task DeleteForwardChannelByIdAsync(long channnelId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="chatId"></param>
        /// <returns></returns>
        Task DeleteForwardChatByIdAsync(long chatId);

    }
}
