using Fishie.Core.Repositories;
using Fishie.Database.Context;
using Fishie.Database.Repositories.Converters;
using Microsoft.EntityFrameworkCore;
using CoreModels = Fishie.Core.Models;
using DbModels = Fishie.Database.Models;

namespace Fishie.Database.Repositories;

public class ChannelRepository : IChannelRepository
{
    private readonly NpgSqlContext _dbContext;

    public ChannelRepository(NpgSqlContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(CoreModels.Channel channel)
    {
        await _dbContext.AddAsync(CoreToDbChannelConverter.Convert(channel)!);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(CoreModels.Channel channel)
    {
        return await _dbContext.Channels.AsNoTracking()
            .AnyAsync(c => c.Id == channel.Id && c.AccessHash == channel.AccessHash);
    }

    public async Task RemoveAsync(string username)
    {
        DbModels.Channel channel = await _dbContext.Channels.FirstAsync(c => c.Username == username);
        _dbContext.Channels.Remove(channel);

        await _dbContext.SaveChangesAsync();
    }

    public async Task RemoveAsync(long id)
    {
        DbModels.Channel channel = await _dbContext.Channels.FirstAsync(c => c.Id == id);
        _dbContext.Channels.Remove(channel);

        await _dbContext.SaveChangesAsync();
    }

    public async Task<List<CoreModels.Channel?>> FindAllAsync()
    {
        return await _dbContext.Channels.AsNoTracking()
            .Select(data => CoreToDbChannelConverter.ConvertBack(data))
            .ToListAsync();
    }

    public async Task<CoreModels.Channel?> FindAsync(string username)
    {
        DbModels.Channel? channel = await _dbContext.Channels.AsNoTracking()
            .FirstOrDefaultAsync(c => c.Username == username);

        return CoreToDbChannelConverter.ConvertBack(channel);
    }

    public async Task<CoreModels.Channel?> FindAsync(long id)
    {
        DbModels.Channel? channel = await _dbContext.Channels.AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);

        return CoreToDbChannelConverter.ConvertBack(channel);
    }
}