using Fishie.Core;
using Fishie.Core.Repositories;
using Fishie.Services.TelegramService.Commands.Utils;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;
using TL;

namespace Fishie.Services.TelegramService.Commands.Unsubscribe
{
    /// <summary>
    /// Unsubscribe to the channel from the database. Example: /unsubscribe channel name
    /// </summary>
    internal class UnsubscribeCommandHandler : AsyncRequestHandler<UnsubscribeCommand>, IDisposable
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IDisposableResource _disposableResource;

        public UnsubscribeCommandHandler(IServiceScopeFactory serviceScopeFactory, IDisposableResource disposableResource)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _disposableResource = disposableResource;
        }

        public void Dispose()
        {
            _disposableResource?.Dispose();
        }

        protected override async Task Handle(UnsubscribeCommand request, CancellationToken cancellationToken)
        {
            string? answer = null;
            if (request.Action!.IndexOf("--info") != -1)
            {
                answer = "Unsubscribe to the channel from the database. Example: /unsubscribe channel name";
            }
            else
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    IChannelRepository chatRepository = scope.ServiceProvider.GetRequiredService<IChannelRepository>();
                    var channel = await chatRepository.FindChannelAsync(request.Action);
                    answer = $"Channels {request.Action} not found in the database";
                    if (channel != null)
                    {
                        await request.Client!.LeaveChat(new InputChannel()
                        {
                            channel_id = channel.Id,
                            access_hash = channel.AccessHash
                        });
                        answer = $"You have unsubscribed to the channel {request.Action}";
                    }
                }
            }

            if (answer != null) await CommandResponseHelper.ExecuteAsync(_serviceScopeFactory, request.Client!,
                (long)(request.ChatId!),
                answer);
        }
    }
}
