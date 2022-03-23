using Fishie.Core.Models;
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
                var isChannelExists = await _dbContext.Chats!.AnyAsync(c => c.Id == chat.Id);

                if (!isChannelExists)
                {
                    await _dbContext.AddAsync(CoreToDbChatConverter.Convert(chat)!);
                    await _dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Repository: {RepositoryName} in Method: {MethodName},",
                    nameof(ChannelRepository),
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
                    throw new Exception($"channel name - {chatName} is not found");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Repository: {RepositoryName} in Method: {MethodName},",
                    nameof(ChannelRepository),
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
                    throw new Exception($"channel id - {id} is not found");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Repository: {RepositoryName} in Method: {MethodName},",
                    nameof(ChannelRepository),
                    nameof(DeleteChatByIdAsync));
            }
        }

        public async Task<IEnumerable<Chat?>?> GetAllChatsAsync()
        {
            try
            {
                IEnumerable<DbModels.Chat?> storedChat = await _dbContext.Chats!.ToListAsync();

                return storedChat.Select(data => CoreToDbChatConverter.ConvertBack(data));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Repository: {RepositoryName} in Method: {MethodName},",
                    nameof(ChannelRepository),
                    nameof(GetAllChatsAsync));
            }

            return null;
        }

        public async Task<Chat?> GetChatAsync(string chatName)
        {
            try
            {
                DbModels.Chat? chat = await _dbContext.Chats!.FirstOrDefaultAsync(c => c.Name == chatName);

                return CoreToDbChatConverter.ConvertBack(chat);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Repository: {RepositoryName} in Method: {MethodName},",
                    nameof(ChannelRepository),
                    nameof(GetChatAsync));
            }

            return null;
        }

        public async Task<Chat?> GetChatByIdAsync(long id)
        {
            try
            {
                DbModels.Chat? channel = await _dbContext.Chats!.FirstOrDefaultAsync(c => c.Id == id);

                return CoreToDbChatConverter.ConvertBack(channel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Repository: {RepositoryName} in Method: {MethodName},",
                    nameof(ChannelRepository),
                    nameof(GetChatByIdAsync));
            }

            return null;
        }
    }
}
