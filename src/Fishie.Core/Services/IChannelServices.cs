using Fishie.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fishie.Core.Services
{
    public interface IChannelServices
    {
        /// <summary>
        /// Adds a channel
        /// </summary>
        /// <param name="channel">Сhannel</param>
        /// <returns></returns>
        Task AddChannelAsync(string channelName);

        /// <summary>
        /// Returns channels
        /// </summary>
        /// <returns>Channels</returns>
        Task<IEnumerable<Channel?>> GetAllChannelsAsync();

        /// <summary>
        /// Returns the channel by name
        /// </summary>
        /// <param name="channelName">Channel name</param>
        /// <returns>Channel</returns>
        Task<Channel?> GetChannelAsync(string channelName);

        /// <summary>
        /// Returns the channel by id
        /// </summary>
        /// <param name="id">Channel id</param>
        /// <returns>Channel</returns>
        Task<Channel?> GetChannelByIdAsync(long id);

        /// <summary>
        /// Deletes a channel by name
        /// </summary>
        /// <param name="channelName">Channel name</param>
        /// <returns>true - channel deleted, false - channel has not been deleted</returns>
        Task<bool> DeleteChannelAsync(string channelName);

        /// <summary>
        /// Deletes a channel by id
        /// </summary>
        /// <param name="id">Channel id</param>
        /// <returns>true - channel deleted, false - channel has not been deleted</returns>
        Task<bool> DeleteChannelByIdAsync(long id);
    }
}
