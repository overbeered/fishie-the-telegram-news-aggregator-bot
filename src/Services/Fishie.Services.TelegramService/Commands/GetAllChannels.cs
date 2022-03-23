using Fishie.Core.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using TL;
using WTelegram;


namespace Fishie.Services.TelegramService.Commands
{
    /// <summary>
    /// Sends a list of channels to the chat. Example: /getAllChannels chat name
    /// </summary>
    internal class GetAllChannels : ICommand
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public GetAllChannels(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task ExecuteAsync(Client client, string action)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                IChannelRepository channalRepository = scope.ServiceProvider.GetRequiredService<IChannelRepository>();
                IChatRepository chatRepository = scope.ServiceProvider.GetRequiredService<IChatRepository>();
                var sendChat = await chatRepository.GetChatAsync(action);
                var listChannels = await channalRepository.GetAllChannelsAsync();
                string message = "";

                if (listChannels != null)
                {
                    foreach (var channel in listChannels)
                    {
                        message += "Id: " + channel!.Id + " Name: " + channel!.Name + " AccessHash: " + channel.AccessHash + "\n";
                    }
                }

                await client.SendMessageAsync(new InputChannel()
                {
                    channel_id = sendChat!.Id,
                    access_hash = sendChat.AccessHash
                }, message);
            }
        }
    }
}
