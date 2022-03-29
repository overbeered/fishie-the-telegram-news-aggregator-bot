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
    public class ChannelRepository : IChannelRepository
    {
        private readonly NpgSqlContext _dbContext;
        private readonly ILogger<ChannelRepository> _logger;

        public ChannelRepository(NpgSqlContext dbContext,
            ILogger<ChannelRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task AddChannelAsync(CoreModels.Channel channel)
        {
            try
            {
                var isChannelExists = await _dbContext.Channels!.AnyAsync(c => c.Id == channel.Id);

                if (!isChannelExists)
                {
                    await _dbContext.AddAsync(CoreToDbChannelConverter.Convert(channel)!);
                    await _dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Repository: {RepositoryName} in Method: {MethodName},",
                    nameof(ChannelRepository),
                    nameof(AddChannelAsync));
            }
        }

        public async Task DeleteChannelAsync(string channelName)
        {
            try
            {
                DbModels.Channel? channel = await _dbContext.Channels!.FirstOrDefaultAsync(c => c.Name == channelName);

                if (channel != null)
                {
                    _dbContext.Channels!.Remove(channel);
                    await _dbContext.SaveChangesAsync();
                }
                else
                {
                    throw new Exception($"channel name - {channelName} is not found");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Repository: {RepositoryName} in Method: {MethodName},",
                    nameof(ChannelRepository),
                    nameof(DeleteChannelAsync));
            }
        }

        public async Task DeleteChannelByIdAsync(long id)
        {
            try
            {
                DbModels.Channel? channel = await _dbContext.Channels!.FirstOrDefaultAsync(c => c.Id == id);

                if (channel != null)
                {
                    _dbContext.Channels!.Remove(channel);
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
                    nameof(DeleteChannelByIdAsync));
            }
        }

        public async Task<IEnumerable<CoreModels.Channel?>?> GetAllChannelsAsync()
        {
            try
            {
                IEnumerable<DbModels.Channel?> storedChannel = await _dbContext.Channels!.ToListAsync();

                return storedChannel.Select(data => CoreToDbChannelConverter.ConvertBack(data));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Repository: {RepositoryName} in Method: {MethodName},",
                    nameof(ChannelRepository),
                    nameof(GetAllChannelsAsync));
            }

            return null;
        }

        public async Task<CoreModels.Channel?> GetChannelAsync(string channelName)
        {
            try
            {
                DbModels.Channel? channel = await _dbContext.Channels!.FirstOrDefaultAsync(c => c.Name == channelName);

                return CoreToDbChannelConverter.ConvertBack(channel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Repository: {RepositoryName} in Method: {MethodName},",
                    nameof(ChannelRepository),
                    nameof(GetChannelAsync));
            }

            return null;
        }

        public async Task<CoreModels.Channel?> GetChannelByIdAsync(long id)
        {
            try
            {
                DbModels.Channel? channel = await _dbContext.Channels!.FirstOrDefaultAsync(c => c.Id == id);

                return CoreToDbChannelConverter.ConvertBack(channel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Repository: {RepositoryName} in Method: {MethodName},",
                    nameof(ChannelRepository),
                    nameof(GetChannelByIdAsync));
            }

            return null;
        }
    }
}
