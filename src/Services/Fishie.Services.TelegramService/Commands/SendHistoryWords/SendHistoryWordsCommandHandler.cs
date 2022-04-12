using Fishie.Core.Repositories;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TL;

namespace Fishie.Services.TelegramService.Commands.SendHistoryWords
{
    /// <summary>
    /// Get the message history from the channel by word. Example: /sendHistoryWords chat name | 5 | words
    /// </summary>
    internal class SendHistoryWordsCommandHandler : AsyncRequestHandler<SendHistoryWordsCommand>
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public SendHistoryWordsCommandHandler(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task Handle(SendHistoryWordsCommand request, CancellationToken cancellationToken)
        {
            if (request.Action!.IndexOf("--info") != -1)
            {
                await ResponseCommand.ExecuteAsync(_serviceScopeFactory,
                    request.Client!,
                    (long)request.ChatId!,
                    "Get the message history from the channel by word. Example: /sendHistoryWords chat name | 5 | words");
            }
            else
            {
                var channelName = request.Action.Remove(request.Action.IndexOf("|") - 1);
                request.Action = request.Action.Remove(0, request.Action.IndexOf("|") + 2);
                var count = int.Parse(request.Action.Remove(request.Action.IndexOf("|") - 1));
                request.Action = request.Action.Remove(0, request.Action.IndexOf("|") + 2);

                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    IChannelRepository channelRepository = scope.ServiceProvider.GetRequiredService<IChannelRepository>();
                    IChatRepository chatRepository = scope.ServiceProvider.GetRequiredService<IChatRepository>();

                    var channel = await channelRepository.GetChannelAsync(channelName);
                    if (channel == null)
                    {
                        await ResponseCommand.ExecuteAsync(_serviceScopeFactory,
                            request.Client!,
                            (long)request.ChatId!,
                            $"channels {channelName} not found in the database");
                    }
                    else
                    {
                        var messagesIdList = new List<int>();

                        var messages = await request.Client.Messages_GetHistory(new InputChannel()
                        {
                            channel_id = channel.Id,
                            access_hash = channel.AccessHash
                        });

                        for (int msgNumber = 0; msgNumber < count; msgNumber++)
                        {
                            var message = (Message)messages.Messages[msgNumber];
                            if (message.message.IndexOf(request.Action) != -1) messagesIdList.Add(message.ID);
                        }

                        var chat = await chatRepository.GetChatByIdAsync((long)request.ChatId!);

                        foreach (var idMessage in messagesIdList)
                        {
                            await request.Client.Messages_ForwardMessages(new InputChannel()
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
