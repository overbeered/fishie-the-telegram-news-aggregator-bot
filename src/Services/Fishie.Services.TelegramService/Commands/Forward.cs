using Fishie.Core.Models;
using Fishie.Core.Repositories;
using Fishie.Services.TelegramService.Commands.ResponseCommands;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using WTelegram;


namespace Fishie.Services.TelegramService.Commands
{
    /// <summary>
    /// Subscribe to message forward. Example: /forward channel name
    /// </summary>
    internal class Forward : ICommand
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public Forward(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }
        public async Task ExecuteAsync(Client client, long chatId, string action)
        {
            if (action.IndexOf("--info") != -1)
            {
                await new ResponseCommand(_serviceScopeFactory).ExecuteAsync(client,
                    chatId,
                    "Subscribe to message forward. Example: /forward channel name");
            }
            else
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    IChannelRepository channelRepository = scope.ServiceProvider.GetRequiredService<IChannelRepository>();
                    IForwardMessagesRepository sendMessagesUpdatesRepository =
                        scope.ServiceProvider.GetRequiredService<IForwardMessagesRepository>();

                    var channel = await channelRepository!.GetChannelAsync(action);

                    if (channel == null)
                    {
                        await new ResponseCommand(_serviceScopeFactory).ExecuteAsync(client,
                            chatId,
                            $"channel {action} not found");
                    }
                    else
                    {
                        await sendMessagesUpdatesRepository.AddForwardMessagesAsync(new ForwardMessages(channel.Id, chatId));
                    }
                }
            }
        }
    }
}
