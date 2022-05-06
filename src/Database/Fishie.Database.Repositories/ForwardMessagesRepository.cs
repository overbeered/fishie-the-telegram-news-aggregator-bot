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
    public class ForwardMessagesRepository : IForwardMessagesRepository
    {
        private readonly NpgSqlContext _dbContext;

        public ForwardMessagesRepository(NpgSqlContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddForwardMessagesAsync(CoreModels.ForwardMessages forwardMessages)
        {
            await _dbContext.AddAsync(CoreToDbSendMessagesUpdatesConverter.Convert(forwardMessages)!);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteForwardMessagesAsync(CoreModels.ForwardMessages forwardMessages)
        {

            DbModels.ForwardMessages model = await _dbContext.ForwardMessages.FirstAsync(c =>
            c.ChannelId == forwardMessages.ChannelId && c.ChatId == forwardMessages.ChatId);
            _dbContext.ForwardMessages.Remove(model);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteForwardChannelByIdAsync(long channnelId)
        {
            var channnes = _dbContext.ForwardMessages.Where(c => c.ChannelId == channnelId);

            if (channnes.Any())
            {
                foreach (var channne in channnes)
                {
                    _dbContext.ForwardMessages.Remove(channne);
                }
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task DeleteForwardChatByIdAsync(long chatId)
        {
            var chats = _dbContext.ForwardMessages.Where(c => c.ChatId == chatId);

            if (chats.Any())
            {
                foreach (var chat in chats)
                {
                    _dbContext.ForwardMessages.Remove(chat);
                }
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<List<CoreModels.ForwardMessages?>> FindAllForwardMessagesAsync()
        {
            return await _dbContext.ForwardMessages.Select(data => CoreToDbSendMessagesUpdatesConverter.ConvertBack(data)).ToListAsync();
        }

        public async Task<bool> ForwardMessagesExistsAsync(CoreModels.ForwardMessages forwardMessages)
        {
            return await _dbContext.ForwardMessages.AnyAsync(f =>
            f.ChannelId == forwardMessages.ChannelId && f.ChatId == forwardMessages.ChatId);
        }
    }
}
