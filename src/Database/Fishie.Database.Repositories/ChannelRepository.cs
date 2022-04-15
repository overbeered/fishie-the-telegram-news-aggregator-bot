using Fishie.Core.Repositories;
using Fishie.Database.Context;
using Fishie.Database.Repositories.Converters;
using Microsoft.EntityFrameworkCore;
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

        public ChannelRepository(NpgSqlContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddChannelAsync(CoreModels.Channel channel)
        {
            await _dbContext.AddAsync(CoreToDbChannelConverter.Convert(channel)!);

            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> ChannelByIdExistsAsync(CoreModels.Channel channel)
        {
            return await _dbContext.Channels.AnyAsync(c => c.Id == channel.Id && c.AccessHash == channel.AccessHash);
        }

        public async Task DeleteChannelAsync(string channelName)
        {
            DbModels.Channel channel = await _dbContext.Channels.FirstAsync(c => c.Name == channelName);
            _dbContext.Channels.Remove(channel);

            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteChannelByIdAsync(long id)
        {
            DbModels.Channel channel = await _dbContext.Channels.FirstAsync(c => c.Id == id);
            _dbContext.Channels.Remove(channel);

            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<CoreModels.Channel?>> FindAllChannelsAsync()
        {
            return await _dbContext.Channels.Select(data => CoreToDbChannelConverter.ConvertBack(data)).ToListAsync();
        }

        public async Task<CoreModels.Channel?> FindChannelAsync(string channelName)
        {
            DbModels.Channel? channel = await _dbContext.Channels.FirstOrDefaultAsync(c => c.Name == channelName);

            return CoreToDbChannelConverter.ConvertBack(channel);
        }

        public async Task<CoreModels.Channel?> FindChannelByIdAsync(long id)
        {
            DbModels.Channel? channel = await _dbContext.Channels.FirstOrDefaultAsync(c => c.Id == id);

            return CoreToDbChannelConverter.ConvertBack(channel);
        }
    }
}
