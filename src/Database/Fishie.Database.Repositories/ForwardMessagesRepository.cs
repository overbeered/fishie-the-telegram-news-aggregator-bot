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
    public class ForwardMessagesRepository : IForwardMessagesRepository
    {
        private readonly NpgSqlContext _dbContext;
        private readonly ILogger<ForwardMessagesRepository> _logger;

        public ForwardMessagesRepository(NpgSqlContext dbContext,
            ILogger<ForwardMessagesRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task AddForwardMessagesAsync(CoreModels.ForwardMessages forwardMessages)
        {
            try
            {
                var isModelExists = await _dbContext.ForwardMessages!.AnyAsync(c => 
                c.ChannelId == forwardMessages.ChannelId && c.ChatId == forwardMessages.ChatId);

                if (!isModelExists)
                {
                    await _dbContext.AddAsync(CoreToDbSendMessagesUpdatesConverter.Convert(forwardMessages)!);
                    await _dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Repository: {RepositoryName} in Method: {MethodName},",
                    nameof(ForwardMessagesRepository),
                    nameof(AddForwardMessagesAsync));
            }
        }

        public async Task DeleteForwardMessagesAsync(CoreModels.ForwardMessages forwardMessages)
        {
            try
            {
                DbModels.ForwardMessages? model = await _dbContext.ForwardMessages!.FirstOrDefaultAsync(c => 
                c.ChannelId == forwardMessages.ChannelId && c.ChatId == forwardMessages.ChatId);

                if (model != null)
                {
                    _dbContext.ForwardMessages!.Remove(model);
                    await _dbContext.SaveChangesAsync();
                }
                else
                {
                    throw new Exception($"channel name - {model} is not found");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Repository: {RepositoryName} in Method: {MethodName},",
                    nameof(ForwardMessagesRepository),
                    nameof(DeleteForwardMessagesAsync));
            }
        }

        public async Task DeleteForwardChannelByIdAsync(long channnelId)
        {
            try
            {
                var channnes = _dbContext.ForwardMessages!.Where(c => c.ChannelId == channnelId);

                if (channnes.Any())
                {
                    foreach (var channne in channnes)
                    {
                        _dbContext.ForwardMessages!.Remove(channne);
                    }
                    await _dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Repository: {RepositoryName} in Method: {MethodName},",
                    nameof(ForwardMessagesRepository),
                    nameof(DeleteForwardChannelByIdAsync));
            }
        }

        public async Task DeleteForwardChatByIdAsync(long chatId)
        {
            try
            {
                var chats = _dbContext.ForwardMessages!.Where(c => c.ChatId == chatId);

                if (chats.Any())
                {
                    foreach (var chat in chats)
                    {
                        _dbContext.ForwardMessages!.Remove(chat);
                    }
                    await _dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Repository: {RepositoryName} in Method: {MethodName},",
                    nameof(ForwardMessagesRepository),
                    nameof(DeleteForwardChatByIdAsync));
            }
        }



        public async Task<IEnumerable<CoreModels.ForwardMessages?>?> GetAllForwardMessagesAsync()
        {
            try
            {
                IEnumerable<DbModels.ForwardMessages?> stored = 
                    await _dbContext.ForwardMessages!.ToListAsync();

                return stored.Select(data => CoreToDbSendMessagesUpdatesConverter.ConvertBack(data));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Repository: {RepositoryName} in Method: {MethodName},",
                    nameof(ForwardMessages),
                    nameof(GetAllForwardMessagesAsync));
            }

            return null;
        }
    }
}
