using Fishie.Core.Repositories;
using Fishie.Services.TelegramService.Commands;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TL;
using WTelegram;

namespace Fishie.Services.TelegramService
{
    /// <summary>
    /// Message Handler
    /// </summary>
    internal class MessagesHandler : AsyncRequestHandler<MessagesRequest>
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IMediator _mediator;
        public MessagesHandler(IMediator mediator, IServiceScopeFactory serviceScopeFactory)
        {
            _mediator = mediator;
            _serviceScopeFactory = serviceScopeFactory;
        }
        protected override async Task Handle(MessagesRequest request, CancellationToken cancellationToken)
        {
            if (request.Message!.IndexOf("/") == 0)
            {
                if (request.UserId != null)
                {
                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        IAdminRepository adminRepository = scope.ServiceProvider.GetRequiredService<IAdminRepository>();
                        if (await adminRepository.AdminByIdExistsAsync(request.UserId.Value))
                        {
                            var message = request.Message.Remove(0, 1);
                            var commandRemove = message.Remove(message.IndexOf(" "));
                            var action = message.Remove(0, message.IndexOf(" ") + 1);

                            switch (commandRemove)
                            {
                                case "addAdmin":
                                    await _mediator.Send(new AddAdminCommand()
                                    {
                                        Client = request.Client,
                                        ChatId = request.ChatId,
                                        Action = action,
                                    });
                                    break;
                                case "addChannel":
                                    await _mediator.Send(new AddChannelCommand()
                                    {
                                        Client = request.Client,
                                        ChatId = request.ChatId,
                                        Action = action,
                                    });
                                    break;
                                case "addChat":
                                    await _mediator.Send(new AddChatCommand()
                                    {
                                        Client = request.Client,
                                        ChatId = request.ChatId,
                                        Action = action,
                                    });
                                    break;
                                case "deleteChat":
                                    await _mediator.Send(new DeleteChannelCommand()
                                    {
                                        Client = request.Client,
                                        ChatId = request.ChatId,
                                        Action = action,
                                    });
                                    break;
                                case "getllChannels":
                                    await _mediator.Send(new GetAllChannelsCommand()
                                    {
                                        Client = request.Client,
                                        ChatId = request.ChatId,
                                        Action = action,
                                    });
                                    break;
                                case "subscribe":
                                    await _mediator.Send(new SubscribeCommand()
                                    {
                                        Client = request.Client,
                                        ChatId = request.ChatId,
                                        Action = action,
                                    });
                                    break;
                                case "unsubscribe":
                                    await _mediator.Send(new UnsubscribeCommand()
                                    {
                                        Client = request.Client,
                                        ChatId = request.ChatId,
                                        Action = action,
                                    });
                                    break;
                                case "sendHistory":
                                    await _mediator.Send(new SendHistoryCommand()
                                    {
                                        Client = request.Client,
                                        ChatId = request.ChatId,
                                        Action = action,
                                    });
                                    break;
                                case "SendHistoryWords":
                                    await _mediator.Send(new SendHistoryWordsCommand()
                                    {
                                        Client = request.Client,
                                        ChatId = request.ChatId,
                                        Action = action,
                                    });
                                    break;
                            }
                        }
                    }
                }
            }
            else
            {
                await ForwardMessagesAsync(request.Client!, (long)request.ChatId!, (int)request.MessageId!);
            }
        }

        /// <summary>
        /// Forward messages to chat
        /// </summary>
        /// <param name="client">Сlient</param>
        /// <param name="channelId">Сhannel\chat id</param>
        /// <param name="idMessage">Message id</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private async Task ForwardMessagesAsync(Client client, long channelId, int idMessage)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                IForwardMessagesRepository forwardMessagesRepository = scope.ServiceProvider
                    .GetRequiredService<IForwardMessagesRepository>();
                var listSendMessages = await forwardMessagesRepository.GetAllForwardMessagesAsync();

                if (listSendMessages != null)
                {
                    listSendMessages = listSendMessages!.Where(c => c!.ChannelId == channelId);
                    IChatRepository chatRepository = scope.ServiceProvider.GetRequiredService<IChatRepository>();
                    IChannelRepository channelRepository = scope.ServiceProvider.GetRequiredService<IChannelRepository>();

                    foreach (var sendMessages in listSendMessages)
                    {
                        var chat = await chatRepository.GetChatByIdAsync(sendMessages!.ChatId);

                        if (chat == null) throw new Exception();

                        var core = await channelRepository.GetChannelByIdAsync(channelId);

                        await client.Messages_ForwardMessages(
                            new InputChannel() { channel_id = core!.Id, access_hash = core.AccessHash },
                            new int[] { idMessage },
                            new long[] { Random.Shared.Next(int.MinValue, int.MaxValue) },
                            new InputChannel() { channel_id = chat.Id, access_hash = chat.AccessHash });
                    }
                }
            }
        }
    }
}
