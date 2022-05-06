using Fishie.Core;
using Fishie.Core.Repositories;
using Fishie.Services.TelegramService.Commands.Utils;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;
using TL;
using CoreModels = Fishie.Core.Models;

namespace Fishie.Services.TelegramService.Commands.AddChat
{
    /// <summary>
    /// Find and add a chat to the database. Example: /addChat chat name
    /// </summary>
    internal class AddChatCommandHandler : AsyncRequestHandler<AddChatCommand>, IDisposable
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IDisposableResource _disposableResource;

        public AddChatCommandHandler(IServiceScopeFactory serviceScopeFactory, IDisposableResource disposableResource)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _disposableResource = disposableResource;
        }

        public void Dispose()
        {
            _disposableResource?.Dispose();
        }

        protected override async Task Handle(AddChatCommand request, CancellationToken cancellationToken)
        {
            string? answer = null;

            if (request.Action!.IndexOf("--info") != -1)
            {
                answer = "Find and add a chat to the database. Example: /addChat chat name";
            }
            else
            {
                var search = await request.Client.Contacts_Search(request.Action);
                if (search.chats.Count == 0)
                {
                    answer = $"chat {request.Action} not found";
                }
                else
                {
                    foreach (var (id, chat) in search.chats)
                    {
                        if (((Channel)chat).username == request.Action || chat.Title == request.Action)
                        {
                            var channel = (InputPeerChannel)chat.ToInputPeer();
                            var coreChat = new CoreModels.Chat(
                                    channel.channel_id,
                                    channel.access_hash,
                                    chat.Title,
                                    ((Channel)chat).username);
                            using (var scope = _serviceScopeFactory.CreateScope())
                            {
                                IChatRepository chatRepository = scope.ServiceProvider.GetRequiredService<IChatRepository>();

                                answer = $"The chat {request.Action} has already been added to the database";
                                if (!await chatRepository.ChatByIdExistsAsync(coreChat))
                                {
                                    await chatRepository!.AddChatAsync(coreChat);
                                    answer = $"The chat {request.Action} has been added to the database";
                                }
                            }
                            break;
                        }
                    }
                }
            }

            if (answer != null) await CommandResponseHelper.ExecuteAsync(_serviceScopeFactory, request.Client!,
                (long)(request.ChatId!),
                answer);
        }
    }
}
