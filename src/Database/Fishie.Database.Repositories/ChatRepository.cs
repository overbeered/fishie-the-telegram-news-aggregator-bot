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
    public class ChatRepository : IChatRepository
    {
        private readonly NpgSqlContext _dbContext;

        public ChatRepository(NpgSqlContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddChatAsync(CoreModels.Chat chat)
        {
            await _dbContext.AddAsync(CoreToDbChatConverter.Convert(chat)!);

            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> ChatByIdExistsAsync(CoreModels.Chat chat)
        {
            return await _dbContext.Chats.AnyAsync(c => c.Id == chat.Id && c.AccessHash == chat.AccessHash);
        }

        public async Task DeleteChatAsync(string chatName)
        {
            DbModels.Chat chat = await _dbContext.Chats.FirstAsync(c => c.Name == chatName);
            _dbContext.Chats.Remove(chat);

            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteChatByIdAsync(long id)
        {
            DbModels.Chat chat = await _dbContext.Chats.FirstAsync(c => c.Id == id);
            _dbContext.Chats.Remove(chat);

            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<CoreModels.Chat?>> FindAllChatsAsync()
        {
            return await _dbContext.Chats.Select(data => CoreToDbChatConverter.ConvertBack(data)).ToListAsync();
        }

        public async Task<CoreModels.Chat?> FindChatAsync(string chatName)
        {
            DbModels.Chat? chat = await _dbContext.Chats.FirstOrDefaultAsync(c => c.Name == chatName);

            return CoreToDbChatConverter.ConvertBack(chat);
        }

        public async Task<CoreModels.Chat?> FindChatByIdAsync(long id)
        {
            DbModels.Chat? chat = await _dbContext.Chats.FirstOrDefaultAsync(c => c.Id == id);

            return CoreToDbChatConverter.ConvertBack(chat);
        }
    }
}
