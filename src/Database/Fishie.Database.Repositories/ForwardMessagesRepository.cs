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

        public async Task AddUpdateAsync(CoreModels.ForwardMessages forwardMessages)
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
                    nameof(AddUpdateAsync));
            }
        }

        public async Task DeleteUpdateAsync(CoreModels.ForwardMessages forwardMessages)
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
                    nameof(DeleteUpdateAsync));
            }
        }

        public async Task<IEnumerable<CoreModels.ForwardMessages?>?> GetAllUpdateAsync()
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
                    nameof(GetAllUpdateAsync));
            }

            return null;
        }
    }
}
