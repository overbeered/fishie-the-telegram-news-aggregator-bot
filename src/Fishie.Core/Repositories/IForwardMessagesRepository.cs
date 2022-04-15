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
        /// <exception>The model ForwardMessages is already stored in the database</exception>
        Task AddForwardMessagesAsync(ForwardMessages forwardMessages);

        /// <summary>
        /// Deletes a model ForwardMessages
        /// </summary>
        /// <param name="forwardMessages">Model ForwardMessages</param>
        /// <exception>Does not find a model ForwardMessages in the database</exception>
        Task DeleteForwardMessagesAsync(ForwardMessages forwardMessages);

        /// <summary>
        /// Returns models ForwardMessages
        /// </summary>
        /// <returns>If it finds it, it will return a ForwardMessages list, if not, a null list</returns>
        Task<List<ForwardMessages?>> FindAllForwardMessagesAsync();

        /// <summary>
        /// Deletes all channel related entries
        /// </summary>
        /// <param name="channnelId">Channel id</param>
        /// <exception>Does not find a model ForwardMessages with this id Channel id the database</exception>
        Task DeleteForwardChannelByIdAsync(long channnelId);

        /// <summary>
        /// Deletes all chat related entries
        /// </summary>
        /// <param name="chatId">Chat id</param>
        /// <exception>Does not find a model ForwardMessages with this id Chat id the database</exception>
        Task DeleteForwardChatByIdAsync(long chatId);
    }
}
