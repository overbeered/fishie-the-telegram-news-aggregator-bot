using Fishie.Core.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using TL;
using WTelegram;


namespace Fishie.Services.TelegramService.Commands
{
    /// <summary>
    /// /getAllChannels Overbeered
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
                IChannelRepository chatRepository = scope.ServiceProvider.GetRequiredService<IChannelRepository>();
                var sendChannel = await chatRepository.GetChannelAsync(action);
                var listChannels = await chatRepository.GetAllChannelsAsync();
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
                    channel_id = sendChannel!.Id,
                    access_hash = sendChannel.AccessHash
                }, message);
            }
        }
    }
}
