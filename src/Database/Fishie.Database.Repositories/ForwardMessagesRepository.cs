using Fishie.Core.Repositories;
using Fishie.Database.Context;
using Fishie.Database.Repositories.Converters;
using Microsoft.EntityFrameworkCore;
using CoreModels = Fishie.Core.Models;
using DbModels = Fishie.Database.Models;

namespace Fishie.Database.Repositories;

public class ForwardMessagesRepository : IForwardMessagesRepository
{
    private readonly NpgSqlContext _dbContext;

    public ForwardMessagesRepository(NpgSqlContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(CoreModels.ForwardMessages forwardMessages)
    {
        await _dbContext.AddAsync(CoreToDbSendMessagesUpdatesConverter.Convert(forwardMessages)!);

        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(CoreModels.ForwardMessages forwardMessages)
    {

        DbModels.ForwardMessages model = await _dbContext.ForwardMessages
            .FirstAsync(c => c.ChannelId == forwardMessages.ChannelId && c.ChatId == forwardMessages.ChatId);
        _dbContext.ForwardMessages.Remove(model);

        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteChannelIdAsync(long channnelId)
    {
        var channnes = _dbContext.ForwardMessages.Where(c => c.ChannelId == channnelId);
        _dbContext.ForwardMessages.RemoveRange(channnes);

        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteChatIdAsync(long chatId)
    {
        var chats = _dbContext.ForwardMessages.Where(c => c.ChatId == chatId);
        _dbContext.ForwardMessages.RemoveRange(chats);

        await _dbContext.SaveChangesAsync();
    }

    public async Task<List<CoreModels.ForwardMessages?>> FindAllAsync()
    {
        return await _dbContext.ForwardMessages.AsNoTracking()
            .Select(data => CoreToDbSendMessagesUpdatesConverter.ConvertBack(data))
            .ToListAsync();
    }

    public async Task<List<CoreModels.ForwardMessages?>> FindChannelIdAsync(long channelId)
    {
        return await _dbContext.ForwardMessages.AsNoTracking()
            .Where(date => date.ChannelId == channelId)
            .Select(data => CoreToDbSendMessagesUpdatesConverter.ConvertBack(data))
            .ToListAsync();
    }

    public async Task<List<CoreModels.ForwardMessages?>> FindChatIdAsync(long chatId)
    {
        return await _dbContext.ForwardMessages.AsNoTracking()
            .Where(date => date.ChatId == chatId)
            .Select(data => CoreToDbSendMessagesUpdatesConverter.ConvertBack(data))
            .ToListAsync();
    }

    public async Task<bool> ExistsAsync(CoreModels.ForwardMessages forwardMessages)
    {
        return await _dbContext.ForwardMessages.AsNoTracking()
            .AnyAsync(f => f.ChannelId == forwardMessages.ChannelId && f.ChatId == forwardMessages.ChatId);
    }

    public async Task<bool> ChatIdExistsAsync(long chatId)
    {
        return await _dbContext.ForwardMessages.AsNoTracking()
            .AnyAsync(f => f.ChatId == chatId);
    }

    public async Task<bool> ChannelIdExistsAsync(long channelId)
    {
        return await _dbContext.ForwardMessages.AsNoTracking()
            .AnyAsync(f => f.ChannelId == channelId);
    }
}