using Fishie.Core.Repositories;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using System.Threading.Tasks;
using TL;

namespace Fishie.Services.TelegramService.Commands.Unsubscribe
{
    /// <summary>
    /// Unsubscribe to the channel from the database. Example: /unsubscribe channel name
    /// </summary>
    internal class UnsubscribeCommandHandler : AsyncRequestHandler<UnsubscribeCommand>
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public UnsubscribeCommandHandler(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task Handle(UnsubscribeCommand request, CancellationToken cancellationToken)
        {
            if (request.Action!.IndexOf("--info") != -1)
            {
                await ResponseCommand.ExecuteAsync(_serviceScopeFactory,
                    request.Client!,
                    (long)request.ChatId!,
                    "Unsubscribe to the channel from the database. Example: /unsubscribe channel name");
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
                        await request.Client!.LeaveChat(new InputChannel()
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
