using Fishie.Core.Repositories;
using Fishie.Database.Context;
using Fishie.Database.Repositories.Converters;
using Microsoft.EntityFrameworkCore;
using CoreModels = Fishie.Core.Models;
using DbModels = Fishie.Database.Models;

namespace Fishie.Database.Repositories;

public class ChatRepository : IChatRepository
{
    private readonly NpgSqlContext _dbContext;

    public ChatRepository(NpgSqlContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(CoreModels.Chat chat)
    {
        await _dbContext.AddAsync(CoreToDbChatConverter.Convert(chat)!);

        await _dbContext.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(CoreModels.Chat chat)
    {
        return await _dbContext.Chats.AsNoTracking()
            .AnyAsync(c => c.Id == chat.Id && c.AccessHash == chat.AccessHash);
    }

    public async Task DeleteAsync(string chatName)
    {
        DbModels.Chat chat = await _dbContext.Chats.FirstAsync(c => c.Name == chatName);
        _dbContext.Chats.Remove(chat);

        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(long id)
    {
        DbModels.Chat chat = await _dbContext.Chats.FirstAsync(c => c.Id == id);
        _dbContext.Chats.Remove(chat);

        await _dbContext.SaveChangesAsync();
    }

    public async Task<List<CoreModels.Chat?>> FindAllAsync()
    {
        return await _dbContext.Chats.AsNoTracking()
            .Select(data => CoreToDbChatConverter.ConvertBack(data))
            .ToListAsync();
    }

    public async Task<CoreModels.Chat?> FindAsync(string chatName)
    {
        DbModels.Chat? chat = await _dbContext.Chats.AsNoTracking()
            .FirstOrDefaultAsync(c => c.Name == chatName);

        return CoreToDbChatConverter.ConvertBack(chat);
    }

    public async Task<CoreModels.Chat?> FindAsync(long id)
    {
        DbModels.Chat? chat = await _dbContext.Chats.AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);

        return CoreToDbChatConverter.ConvertBack(chat);
    }
}