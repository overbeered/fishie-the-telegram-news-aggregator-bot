using Fishie.Core.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using WTelegram;
using TL;

namespace Fishie.Services.TelegramService.Commands
{
    /// <summary>
    /// Unsubscribe to the channel\chat from the database. Example: /unsubscribe channel name
    /// </summary>
    internal class Unsubscribe : ICommand
    {
            
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public Unsubscribe(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }
        public async Task ExecuteAsync(Client client, string action)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                IChannelRepository chatRepository = scope.ServiceProvider.GetRequiredService<IChannelRepository>();
                var channel = await chatRepository.GetChannelAsync(action);

                if (channel == null) throw new Exception();

                await client.LeaveChat(new InputChannel()
                {
                    channel_id = channel.Id,
                    access_hash = channel.AccessHash
                });
            }
        }
    }
}
