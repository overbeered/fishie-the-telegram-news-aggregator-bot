﻿using Fishie.Core;
using Fishie.Core.Repositories;
using Fishie.Services.TelegramService.Commands.Utils;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;
using TL;
using CoreModels = Fishie.Core.Models;

namespace Fishie.Services.TelegramService.Commands.AddChannel
{
    /// <summary>
    /// Find and add a channel\chat to the database. Example: /addChannel channel name
    /// </summary>
    internal class AddChannelCommandHandler : AsyncRequestHandler<AddChannelCommand>, IDisposable
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IDisposableResource _disposableResource;

        public AddChannelCommandHandler(IServiceScopeFactory serviceScopeFactory, IDisposableResource disposableResource)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _disposableResource = disposableResource;
        }

        public void Dispose()
        {
            _disposableResource?.Dispose();
        }

        protected override async Task Handle(AddChannelCommand request, CancellationToken cancellationToken)
        {
            string? answer = null;

            if (request.Action!.IndexOf("--info") != -1)
            {
                answer = "Find and add a channel\\chat to the database. Example: /addChannel channel name";
            }
            else
            {
                var search = await request.Client.Contacts_Search(request.Action);
                if (search.chats.Count == 0)
                {
                    answer = $"channel {request.Action} not found";
                }
                else
                {
                    foreach (var (_, chat) in search.chats)
                    {
                        if (chat.IsActive && (((Channel)chat).username == request.Action || chat.Title == request.Action))
                        {
                            var channel = (InputPeerChannel)chat.ToInputPeer();
                            var coreChannel = new CoreModels.Channel(
                                    channel.channel_id,
                                    channel.access_hash,
                                    chat.Title,
                                    ((Channel)chat).username);

                            using (var scope = _serviceScopeFactory.CreateScope())
                            {
                                IChannelRepository channelRepository = scope.ServiceProvider.GetRequiredService<IChannelRepository>();

                                answer = $"The channel {request.Action} has already been added to the database";

                                if (!await channelRepository.ChannelByIdExistsAsync(coreChannel))
                                {
                                    await channelRepository!.AddChannelAsync(coreChannel);
                                    answer = $"The channel {request.Action} has been added to the database";
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
