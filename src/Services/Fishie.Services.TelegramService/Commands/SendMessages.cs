using Fishie.Core.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using TL;
using WTelegram;

namespace Fishie.Services.TelegramService.Commands
{
    /// <summary>
    /// Send a message to the channel\chat from the database. Example: /sendMessages channel name message: Hello world!
    /// </summary>
    internal class SendMessages : ICommand
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public SendMessages(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task ExecuteAsync(Client client, string action)
        {
            var channelName = action.Remove(action.IndexOf("message: ") - 1);
            var message = action.Remove(0, action.IndexOf("message: ") + 9);

            using (var scope = _serviceScopeFactory.CreateScope())
            {
                IChannelRepository chatRepository = scope.ServiceProvider.GetRequiredService<IChannelRepository>();
                var channel = await chatRepository.GetChannelAsync(channelName);

                if (channel == null) throw new Exception();

                await client.SendMessageAsync(new InputChannel()
                {
                    channel_id = channel.Id,
                    access_hash = channel.AccessHash
                }, message);
            }

        }
    }
}
