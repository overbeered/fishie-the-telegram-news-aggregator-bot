using Fishie.Core.Models;
using Fishie.Core.Repositories;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using System.Threading.Tasks;

namespace Fishie.Services.TelegramService.Commands.Forward
{
    /// <summary>
    /// Subscribe to message forward. Example: /forward channel name
    /// </summary>
    internal class ForwardCommandHandler : AsyncRequestHandler<ForwardCommand>
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public ForwardCommandHandler(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task Handle(ForwardCommand request, CancellationToken cancellationToken)
        {
            if (request.Action!.IndexOf("--info") != -1)
            {
                await ResponseCommand.ExecuteAsync(_serviceScopeFactory,
                    request.Client!,
                    (long)request.ChatId!,
                    "Subscribe to message forward. Example: /forward channel name");
            }
            else
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    IChannelRepository channelRepository = scope.ServiceProvider.GetRequiredService<IChannelRepository>();
                    IForwardMessagesRepository sendMessagesUpdatesRepository =
                        scope.ServiceProvider.GetRequiredService<IForwardMessagesRepository>();

                    var channel = await channelRepository!.GetChannelAsync(request.Action);

                    if (channel == null)
                    {
                        await ResponseCommand.ExecuteAsync(_serviceScopeFactory,
                            request.Client!,
                            (long)request.ChatId!,
                            $"Channel {request.Action} not found");
                    }
                    else
                    {
                        await sendMessagesUpdatesRepository.AddForwardMessagesAsync(new ForwardMessages(channel.Id, (long)request.ChatId!));
                    }
                }
            }
        }
    }
}
