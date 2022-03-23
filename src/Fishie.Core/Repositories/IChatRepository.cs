using Fishie.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fishie.Core.Repositories
{
    /// <summary>
    /// Defines chat repository interface.
    /// </summary>
    public interface IChatRepository
    {
        /// <summary>
        /// Adds a chat
        /// </summary>
        /// <param name="chat">Chat</param>
        /// <returns></returns>
        Task AddChatAsync(Chat chat);

        /// <summary>
        /// Returns chats
        /// </summary>
        /// <returns>Chats</returns>
        Task<IEnumerable<Chat?>?> GetAllChatsAsync();

        /// <summary>
        /// Returns the chat by name
        /// </summary>
        /// <param name="chatName">Chat name</param>
        /// <returns>Chat</returns>
        Task<Chat?> GetChatAsync(string chatName);

        /// <summary>
        /// Returns the chat by id
        /// </summary>
        /// <param name="id">Chat id</param>
        /// <returns>Chat</returns>
        Task<Chat?> GetChatByIdAsync(long id);

        /// <summary>
        /// Deletes a chat by name
        /// </summary>
        /// <param name="chatName">Chat name</param>
        /// <returns></returns>
        Task DeleteChatAsync(string chatName);

        /// <summary>
        /// Deletes a chat by id
        /// </summary>
        /// <param name="id">Chat id</param>
        /// <returns></returns>
        Task DeleteChatByIdAsync(long id);
    }
}
