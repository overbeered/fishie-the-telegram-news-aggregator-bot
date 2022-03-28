using Fishie.Core.Repositories;
using Fishie.Services.TelegramService.Commands.ResponseCommands;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using TL;
using WTelegram;

namespace Fishie.Services.TelegramService.Commands
{
    /// <summary>
    /// Unsubscribe to the channel from the database. Example: /unsubscribe channel name
    /// </summary>
    internal class Unsubscribe : ICommand
    {

        private readonly IServiceScopeFactory _serviceScopeFactory;

        public Unsubscribe(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }
        public async Task ExecuteAsync(Client client, long chatId, string action)
        {
            if (action.IndexOf("--info") != -1)
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    await new ResponseCommand(_serviceScopeFactory).ExecuteAsync(client,
                        chatId,
                        "Unsubscribe to the channel from the database. Example: /unsubscribe channel name");
                }
            }
            else
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    IChannelRepository chatRepository = scope.ServiceProvider.GetRequiredService<IChannelRepository>();
                    var channel = await chatRepository.GetChannelAsync(action);

                    if (channel == null)
                    {
                        await new ResponseCommand(_serviceScopeFactory).ExecuteAsync(client,
                            chatId,
                            $"channels {action} not found in the database");
                    }
                    else
                    {
                        await client.LeaveChat(new InputChannel()
                        {
                            channel_id = channel.Id,
                            access_hash = channel.AccessHash
                        });
                    }
                }
            }
        }
    }
}
