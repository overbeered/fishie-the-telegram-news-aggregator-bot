using Fishie.Core.Repositories;
using Fishie.Services.TelegramService.Commands.ResponseCommands;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Threading.Tasks;
using WTelegram;

namespace Fishie.Services.TelegramService.Commands
{
    /// <summary>
    /// Delete from the database channel\chat. Example: /deleteChannel channel name
    /// </summary>
    internal class DeleteToChannel : ICommand
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public DeleteToChannel(IServiceScopeFactory serviceScopeFactory)
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
                        "Delete from the database channel\\chat. Example: /deleteChannel channel name");
                }
            }
            else
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    IChannelRepository channelRepository = scope.ServiceProvider.GetRequiredService<IChannelRepository>();
                    IForwardMessagesRepository forwardMessagesRepository = scope.ServiceProvider.GetRequiredService<IForwardMessagesRepository>();
                    var channel = await channelRepository.GetChannelAsync(action);
                    if (channel != null)
                    {
                        await channelRepository.DeleteChannelAsync(action);
                        await forwardMessagesRepository.DeleteForwardChannelByIdAsync(channel.Id);
                    }
                }
            }
        }
    }
}
