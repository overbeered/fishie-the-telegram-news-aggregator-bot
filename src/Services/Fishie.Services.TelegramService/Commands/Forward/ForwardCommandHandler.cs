using Fishie.Core;
using Fishie.Core.Models;
using Fishie.Core.Repositories;
using Fishie.Services.TelegramService.Commands.Utils;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Fishie.Services.TelegramService.Commands.Forward
{
    /// <summary>
    /// Subscribe to message forward. Example: /forward channel name
    /// </summary>
    internal class ForwardCommandHandler : AsyncRequestHandler<ForwardCommand>, IDisposable
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IDisposableResource _disposableResource;

        public ForwardCommandHandler(IServiceScopeFactory serviceScopeFactory, IDisposableResource disposableResource)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _disposableResource = disposableResource;
        }

        public void Dispose()
        {
            _disposableResource?.Dispose();
        }

        protected override async Task Handle(ForwardCommand request, CancellationToken cancellationToken)
        {
            string? answer = null;

            if (request.Action!.IndexOf("--info") != -1)
            {
                answer = "Subscribe to message forward. Example: /forward channel name";
            }
            else
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    IChannelRepository channelRepository = scope.ServiceProvider.GetRequiredService<IChannelRepository>();
                    IForwardMessagesRepository forwardMessagesRepository =
                        scope.ServiceProvider.GetRequiredService<IForwardMessagesRepository>();

                    var channel = await channelRepository!.FindChannelAsync(request.Action);
                    answer = $"Channel {request.Action} not found in the database";

                    if (channel != null)
                    {
                        if (!await forwardMessagesRepository.ForwardMessagesExistsAsync(new ForwardMessages(channel.Id, (long)request.ChatId!)))
                        {
                            await forwardMessagesRepository.AddForwardMessagesAsync(new ForwardMessages(channel.Id, (long)request.ChatId!));
                            answer = $"You are subscribed to channel updates {request.Action}";
                        }
                        else
                            answer = $"You have already subscribed to message updates for this channel {request.Action}";
                    }
                }
            }

            if (answer != null) await CommandResponseHelper.ExecuteAsync(_serviceScopeFactory, request.Client!,
                (long)(request.ChatId!),
                answer);
        }
    }
}
