using Fishie.Core.Repositories;
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
            if (request.Action!.IndexOf("--info") != -1)
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    await ResponseCommand.ExecuteAsync(_serviceScopeFactory,
                        request.Client!,
                        (long)request.ChatId!,
                        "Subscribe to the channel from the database. Example: /subscribe channel name");
                }
            }
            else
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    IChannelRepository chatRepository = scope.ServiceProvider.GetRequiredService<IChannelRepository>();
                    var channel = await chatRepository.GetChannelAsync(request.Action);

                    if (channel == null)
                    {
                        await ResponseCommand.ExecuteAsync(_serviceScopeFactory,
                            request.Client!,
                            (long)request.ChatId!,
                            $"channels {request.Action} not found in the database");
                    }
                    else
                    {
                        await request.Client.Channels_JoinChannel(new InputChannel()
                        {
                            channel_id = channel.Id,
                            access_hash = channel.AccessHash
                        });
                    }
                }
            }
        }
    }
}
