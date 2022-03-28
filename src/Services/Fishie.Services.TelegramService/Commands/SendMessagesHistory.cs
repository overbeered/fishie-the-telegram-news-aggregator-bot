using Fishie.Core.Repositories;
using Fishie.Services.TelegramService.Commands.ResponseCommands;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TL;
using WTelegram;

namespace Fishie.Services.TelegramService.Commands
{
    /// <summary>
    /// Get the message history from the channel.  Example: /sendMessagesHistory chat name | 5
    /// </summary>
    internal class SendMessagesHistory : ICommand
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public SendMessagesHistory(IServiceScopeFactory serviceScopeFactory)
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
                        "Get the message history from the channel.  Example: /sendMessagesHistory chat name | 5");
                }
            }
            else
            {
                var channelName = action.Remove(action.IndexOf("|") - 1);
                var count = int.Parse(action.Remove(0, action.IndexOf("|") + 2));

                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    IChannelRepository channelRepository = scope.ServiceProvider.GetRequiredService<IChannelRepository>();
                    IChatRepository chatRepository = scope.ServiceProvider.GetRequiredService<IChatRepository>();

                    var channel = await channelRepository.GetChannelAsync(channelName);
                    if (channel == null)
                    {
                        await new ResponseCommand(_serviceScopeFactory).ExecuteAsync(client,
                            chatId,
                            $"channels {channelName} not found in the database");
                    }
                    else
                    {
                        var messagesIdList = new List<int>();

                        var messages = await client.Messages_GetHistory(new InputChannel()
                        {
                            channel_id = channel.Id,
                            access_hash = channel.AccessHash
                        });

                        for (int msgNumber = 0; msgNumber < count; msgNumber++)
                        {
                            var message = (Message)messages.Messages[msgNumber];
                            messagesIdList.Add(message.ID);
                        }

                        var chat = await chatRepository.GetChatByIdAsync(chatId);

                        foreach (var idMessage in messagesIdList)
                        {
                            await client.Messages_ForwardMessages(new InputChannel()
                            { channel_id = channel.Id, access_hash = channel.AccessHash },
                            new int[] { idMessage },
                            new long[] { Random.Shared.Next(int.MinValue, int.MaxValue) },
                            new InputChannel()
                            { channel_id = chat!.Id, access_hash = chat.AccessHash });
                        }
                    }
                }
            }
        }
    }
}
