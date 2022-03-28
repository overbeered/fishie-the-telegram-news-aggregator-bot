using Fishie.Core.Repositories;
using Fishie.Services.TelegramService.Commands.ResponseCommands;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Threading.Tasks;
using WTelegram;


namespace Fishie.Services.TelegramService.Commands
{
    /// <summary>
    /// Sends a list of channels to the chat. Example: /getAllChannels
    /// </summary>
    internal class GetAllChannels : ICommand
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public GetAllChannels(IServiceScopeFactory serviceScopeFactory)
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
                        "Sends a list of channels to the chat. Example: /getAllChannels");
                }
            }
            else
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    IChannelRepository channalRepository = scope.ServiceProvider.GetRequiredService<IChannelRepository>();
                    var listChannels = await channalRepository.GetAllChannelsAsync();
                    string message = "";

                    if (listChannels!.Count() != 0)
                    {
                        foreach (var channel in listChannels!)
                        {
                            message += "Id: " + channel!.Id + " Name: " + channel!.Name + " AccessHash: " + channel.AccessHash + "\n";
                        }

                        await new ResponseCommand(_serviceScopeFactory).ExecuteAsync(client,
                            chatId,
                            message);
                    }
                    else
                    {
                        await new ResponseCommand(_serviceScopeFactory).ExecuteAsync(client,
                            chatId,
                            "Channels not found");
                    }
                }
            }
        }
    }
}
