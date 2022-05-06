using Fishie.Core;
using Fishie.Core.Repositories;
using Fishie.Services.TelegramService.Commands.Utils;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Fishie.Services.TelegramService.Commands.DeleteChannelForward
{
    /// <summary>
    /// Remove channel tracking. Example: /deleteChannelForward channel name
    /// </summary>
    internal class DeleteChannelForwardCommandHandler : AsyncRequestHandler<DeleteChannelForwardCommand>, IDisposable
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IDisposableResource _disposableResource;

        public DeleteChannelForwardCommandHandler(IServiceScopeFactory serviceScopeFactory, IDisposableResource disposableResource)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _disposableResource = disposableResource;
        }

        public void Dispose()
        {
            _disposableResource?.Dispose();
        }

        protected override async Task Handle(DeleteChannelForwardCommand request, CancellationToken cancellationToken)
        {
            string? answer = null;

            if (request.Action!.IndexOf("--info") != -1)
            {
                answer = "Remove channel tracking. Example: /deleteChannelForward channel name";
            }
            else
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    IChannelRepository channelRepository = scope.ServiceProvider.GetRequiredService<IChannelRepository>();
                    IForwardMessagesRepository forwardMessagesRepository = scope.ServiceProvider.GetRequiredService<IForwardMessagesRepository>();
                    var channel = await channelRepository.FindChannelAsync(request.Action);
                    if (channel != null)
                    {
                        var listSendMessages = await forwardMessagesRepository.FindAllForwardMessagesAsync();
                        var forwardMessages = listSendMessages!.FirstOrDefault(c =>
                        c!.ChatId == request.ChatId && c.ChannelId == channel.Id);

                        answer = $"The channel {request.Action} not stored in the surveillance database";
                        if (forwardMessages != null)
                        {
                            await forwardMessagesRepository.DeleteForwardMessagesAsync(forwardMessages);
                            answer = $"The channel {request.Action} was removed from tracking";
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
