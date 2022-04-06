using Fishie.Core.Repositories;
using Fishie.Database.Context;
using Fishie.Database.Repositories.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreModels = Fishie.Core.Models;
using DbModels = Fishie.Database.Models;

namespace Fishie.Database.Repositories
{
    public class ChatRepository : IChatRepository
    {
        private readonly NpgSqlContext _dbContext;
        private readonly ILogger<ChatRepository> _logger;

        public ChatRepository(NpgSqlContext dbContext,
            ILogger<ChatRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task AddChatAsync(CoreModels.Chat chat)
        {
            try
            {
                var isChatExists = await _dbContext.Chats!.AnyAsync(c => c.Id == chat.Id);

                if (!isChatExists)
                {
                    await _dbContext.AddAsync(CoreToDbChatConverter.Convert(chat)!);
                    await _dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Repository: {RepositoryName} in Method: {MethodName},",
                    nameof(ChatRepository),
                    nameof(AddChatAsync));
            }
        }

        public async Task DeleteChatAsync(string chatName)
        {
            try
            {
                DbModels.Chat? chat = await _dbContext.Chats!.FirstOrDefaultAsync(c => c.Name == chatName);

                if (chat != null)
                {
                    _dbContext.Chats!.Remove(chat);
                    await _dbContext.SaveChangesAsync();
                }
                else
                {
                    throw new Exception($"chat name - {chatName} is not found");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Repository: {RepositoryName} in Method: {MethodName},",
                    nameof(ChatRepository),
                    nameof(DeleteChatAsync));
            }
        }

        public async Task DeleteChatByIdAsync(long id)
        {
            try
            {
                DbModels.Chat? chat = await _dbContext.Chats!.FirstOrDefaultAsync(c => c.Id == id);

                if (chat != null)
                {
                    _dbContext.Chats!.Remove(chat);
                    await _dbContext.SaveChangesAsync();
                }
                else
                {
                    throw new Exception($"chat id - {id} is not found");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Repository: {RepositoryName} in Method: {MethodName},",
                    nameof(ChatRepository),
                    nameof(DeleteChatByIdAsync));
            }
        }

        public async Task<IEnumerable<CoreModels.Chat?>?> GetAllChatsAsync()
        {
            try
            {
                IEnumerable<DbModels.Chat?> storedChat = await _dbContext.Chats!.ToListAsync();

                return storedChat.Select(data => CoreToDbChatConverter.ConvertBack(data));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Repository: {RepositoryName} in Method: {MethodName},",
                    nameof(ChatRepository),
                    nameof(GetAllChatsAsync));
            }

            return null;
        }

        public async Task<CoreModels.Chat?> GetChatAsync(string chatName)
        {
            try
            {
                DbModels.Chat? chat = await _dbContext.Chats!.FirstOrDefaultAsync(c => c.Name == chatName);

                return CoreToDbChatConverter.ConvertBack(chat);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Repository: {RepositoryName} in Method: {MethodName},",
                    nameof(ChatRepository),
                    nameof(GetChatAsync));
            }

            return null;
        }

        public async Task<CoreModels.Chat?> GetChatByIdAsync(long id)
        {
            try
            {
                DbModels.Chat? chat = await _dbContext.Chats!.FirstOrDefaultAsync(c => c.Id == id);

                return CoreToDbChatConverter.ConvertBack(chat);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Repository: {RepositoryName} in Method: {MethodName},",
                    nameof(ChatRepository),
                    nameof(GetChatByIdAsync));
            }

            return null;
        }
    }
}
