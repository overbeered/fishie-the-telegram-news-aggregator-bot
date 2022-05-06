using Fishie.Core;
using Fishie.Core.Repositories;
using Fishie.Services.TelegramService.Commands;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
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
    internal class MessagesHandler : AsyncRequestHandler<MessagesRequest>, IDisposable
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IMediator _mediator;
        private readonly Dictionary<string, Command> _handlers;
        private readonly IDisposableResource _disposableResource;
        public MessagesHandler(IMediator mediator, IServiceScopeFactory serviceScopeFactory, IDisposableResource disposableResource)
        {
            _mediator = mediator;
            _serviceScopeFactory = serviceScopeFactory;
            _handlers = new Dictionary<string, Command>()
            {
                {SendCommand.CommandText, new SendCommand()},
                {AddAdminCommand.CommandText, new AddAdminCommand()},
                {AddChannelCommand.CommandText, new AddChannelCommand()},
                {AddChatCommand.CommandText, new AddChatCommand()},
                {DeleteChannelCommand.CommandText, new DeleteChannelCommand()},
                {GetAllChannelsCommand.CommandText, new GetAllChannelsCommand()},
                {SubscribeCommand.CommandText, new SubscribeCommand()},
                {UnsubscribeCommand.CommandText, new UnsubscribeCommand()},
                {SendHistoryCommand.CommandText, new SendHistoryCommand()},
                {SendHistoryWordsCommand.CommandText, new SendHistoryWordsCommand()},
                {ForwardCommand.CommandText, new ForwardCommand()},
                {GetAllForwardCommand.CommandText, new GetAllForwardCommand()},
                {DeleteChannelForwardCommand.CommandText, new DeleteChannelForwardCommand()},
            };
            _disposableResource = disposableResource;
        }

        public void Dispose()
        {
            _disposableResource?.Dispose();
        }

        protected override async Task Handle(MessagesRequest request, CancellationToken cancellationToken)
        {
            try
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
                                string message = request.Message!.Remove(0, 1);
                                string command = message.IndexOf(" ") != -1 ? message.Remove(message.IndexOf(" ")) : message;
                                string? action = command != message ? message.Remove(0, message.IndexOf(" ") + 1) : null;

                                if (command == "commands")
                                {
                                    action = " ";

                                    foreach (var key in _handlers.Keys)
                                    {
                                        action += "\n" + key;
                                    }
                                }
                                _handlers[command].Client = request.Client;
                                _handlers[command].ChatId = request.ChatId;
                                _handlers[command].Action = action;
                                await _mediator.Send(_handlers[command]);
                            }
                        }
                    }
                }
                else
                {
                    await ForwardMessagesAsync(request.Client!, (long)request.ChatId!, (int)request.MessageId!);
                }
            }
            catch (KeyNotFoundException)
            {
                await _mediator.Send(new SendCommand()
                {
                    Client = request.Client,
                    ChatId = request.ChatId,
                    Action = $"Сommand \"{request.Message}\" not found"
                });
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
                var listSendMessages = await forwardMessagesRepository.FindAllForwardMessagesAsync();

                if (listSendMessages != null)
                {
                    listSendMessages = listSendMessages.Where(c => c!.ChannelId == channelId).ToList();
                    IChatRepository chatRepository = scope.ServiceProvider.GetRequiredService<IChatRepository>();
                    IChannelRepository channelRepository = scope.ServiceProvider.GetRequiredService<IChannelRepository>();

                    foreach (var sendMessages in listSendMessages)
                    {
                        var chat = await chatRepository.FindChatByIdAsync(sendMessages!.ChatId);

                        if (chat == null) throw new Exception();

                        var core = await channelRepository.FindChannelByIdAsync(channelId);

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
