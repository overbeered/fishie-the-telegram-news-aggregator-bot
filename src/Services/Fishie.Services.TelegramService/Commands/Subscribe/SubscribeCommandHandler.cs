using Fishie.Core.Repositories;
using Fishie.Services.TelegramService.Commands.Utils;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using System.Threading.Tasks;
using TL;

namespace Fishie.Services.TelegramService.Commands.Subscribe
{
    /// <summary>
    /// Subscribe to the channel from the database. Example: /subscribe channel name
    /// </summary>
    internal class SubscribeCommandHandler : AsyncRequestHandler<SubscribeCommand>
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public SubscribeCommandHandler(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task Handle(SubscribeCommand request, CancellationToken cancellationToken)
        {
            string? answer = null;
            if (request.Action!.IndexOf("--info") != -1)
            {
                answer = "Subscribe to the channel from the database. Example: /subscribe channel name";
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
                        await request.Client.Channels_JoinChannel(new InputChannel()
                        {
                            channel_id = channel.Id,
                            access_hash = channel.AccessHash
                        });

                        answer = $"You have subscribed to the channel {request.Action}";
                    }
                }
            }

            if (answer != null) await CommandResponseHelper.ExecuteAsync(_serviceScopeFactory, request.Client!,
                (long)(request.ChatId!),
                answer);
        }
    }
}
