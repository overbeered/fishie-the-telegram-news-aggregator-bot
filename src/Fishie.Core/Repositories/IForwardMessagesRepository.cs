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
        Task AddUpdateAsync(ForwardMessages forwardMessages);

        /// <summary>
        /// Deletes a model ForwardMessages
        /// </summary>
        /// <param name="forwardMessages">Model ForwardMessages</param>
        /// <returns></returns>
        Task DeleteUpdateAsync(ForwardMessages forwardMessages);

        /// <summary>
        /// Returns models ForwardMessages
        /// </summary>
        /// <returns>Model ForwardMessages</returns>
        Task<IEnumerable<ForwardMessages?>?> GetAllUpdateAsync();
    }
}
