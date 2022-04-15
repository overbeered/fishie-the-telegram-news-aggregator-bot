using Fishie.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fishie.Core.Repositories
{
    /// <summary>
    /// Defines Channel repository interface.
    /// </summary>
    public interface IChannelRepository
    {
        /// <summary>
        /// Adds a channel
        /// </summary>
        /// <param name="channel">Сhannel</param>
        /// <exception>The channel is already stored in the database</exception>
        Task AddChannelAsync(Channel channel);

        /// <summary>
        /// Returns channels
        /// </summary>
        /// <returns>If it finds it, it will return a Channel list, if not, a null list</returns>
        Task<List<Channel?>> FindAllChannelsAsync();

        /// <summary>
        /// Returns the channel by name
        /// </summary>
        /// <param name="channelName">Channel name</param>
        /// <returns>If it finds it, it will return - Channel, if not - null</returns>
        Task<Channel?> FindChannelAsync(string channelName);

        /// <summary>
        /// Returns the channel by id
        /// </summary>
        /// <param name="id">Channel id</param>
        /// <returns>If it finds it, it will return - Channel, if not - null</returns>
        Task<Channel?> FindChannelByIdAsync(long id);

        /// <summary>
        /// Deletes a channel by name
        /// </summary>
        /// <param name="channelName">Channel name</param>
        /// <exception>Does not find a channel with this name in the database</exception>
        Task DeleteChannelAsync(string channelName);

        /// <summary>
        /// Deletes a channel by id
        /// </summary>
        /// <param name="id">Channel id</param>
        /// <exception>Does not find a channel with this id in the database</exception>
        Task DeleteChannelByIdAsync(long id);

        /// <summary>
        /// Checking availability channel
        /// </summary>
        /// <param name="channel">Channel</param>
        /// <returns>Returns true if at least one element of the database is defined by the condition</returns>
        Task<bool> ChannelByIdExistsAsync(Channel channel);
    }
}
