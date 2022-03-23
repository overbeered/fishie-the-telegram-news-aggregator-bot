using Fishie.Core.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WTelegram;
using TL;

namespace Fishie.Services.TelegramService.Commands
{
    /// <summary>
    /// Send the history of recent messages from the channel\chat to Overbeared. Example: /sendMessagesOverbeered channel name | 3
    /// </summary>
    internal class SendMessagesOverbeered : ICommand
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public SendMessagesOverbeered(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }


        public async Task ExecuteAsync(Client client, string action)
        {
            var channelName = action.Remove(action.IndexOf("|") - 1);
            var count = int.Parse(action.Remove(0, action.IndexOf("|") + 2));
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                IChannelRepository chatRepository = scope.ServiceProvider.GetRequiredService<IChannelRepository>();
                var channel = await chatRepository.GetChannelAsync(channelName);

                if (channel == null) throw new Exception();

                var messagesList = new List<string?>();

                var messages = await client.Messages_GetHistory(new InputChannel()
                {
                    channel_id = channel.Id,
                    access_hash = channel.AccessHash
                });

                for (int msgNumber = 0; msgNumber < count; msgNumber++)
                {
                    var message = (Message)messages.Messages[msgNumber];
                    messagesList.Add(message.message + " " + "Date:" + message.Date);
                }

                if (messagesList != null)
                {
                    channel = await chatRepository.GetChannelAsync("Overbeered");
                    foreach (var message in messagesList)
                    {
                        await client.SendMessageAsync(new InputChannel()
                        {
                            channel_id = channel!.Id,
                            access_hash = channel.AccessHash
                        }, message);
                    }
                }
               
            }   
        }
    }
}
