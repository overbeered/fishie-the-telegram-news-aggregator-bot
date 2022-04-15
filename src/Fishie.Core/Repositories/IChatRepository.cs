﻿using Fishie.Core.Models;
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
        /// <exception>The chat is already stored in the database</exception>
        Task AddChatAsync(Chat chat);

        /// <summary>
        /// Returns chats
        /// </summary>
        /// <returns>If it finds it, it will return a Chat list, if not, a null list</returns>
        Task<List<Chat?>> FindAllChatsAsync();

        /// <summary>
        /// Returns the chat by name
        /// </summary>
        /// <param name="chatName">Chat name</param>
        /// <returns>If it finds it, it will return - Chat, if not - null</returns>
        Task<Chat?> FindChatAsync(string chatName);

        /// <summary>
        /// Returns the chat by id
        /// </summary>
        /// <param name="id">Chat id</param>
        /// <returns>If it finds it, it will return - Chat, if not - null</returns>
        Task<Chat?> FindChatByIdAsync(long id);

        /// <summary>
        /// Deletes a chat by name
        /// </summary>
        /// <param name="chatName">Chat name</param>
        /// <exception>Does not find a chat with this name in the database</exception>
        Task DeleteChatAsync(string chatName);

        /// <summary>
        /// Deletes a chat by id
        /// </summary>
        /// <param name="id">Chat id</param>
        /// <exception>Does not find a chat with this id in the database</exception>
        Task DeleteChatByIdAsync(long id);

        /// <summary>
        /// Checking availability chat
        /// </summary>
        /// <param name="chat">Chat</param>
        /// <returns>Returns true if at least one element of the database is defined by the condition</returns>
        Task<bool> ChatByIdExistsAsync(Chat chat);
    }
}
