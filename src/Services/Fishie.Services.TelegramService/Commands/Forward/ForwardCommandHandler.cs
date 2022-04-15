using Fishie.Core.Models;
using Fishie.Core.Repositories;
using Fishie.Services.TelegramService.Commands.Utils;
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
                    IForwardMessagesRepository sendMessagesUpdatesRepository =
                        scope.ServiceProvider.GetRequiredService<IForwardMessagesRepository>();

                    var channel = await channelRepository!.FindChannelAsync(request.Action);
                    answer = $"Channel {request.Action} not found in the database";
                    
                    if (channel != null)
                    {

                        await sendMessagesUpdatesRepository.AddForwardMessagesAsync(new ForwardMessages(channel.Id, (long)request.ChatId!));
                        answer = $"You are subscribed to channel updates {request.Action}";
                    }
                }
            }

            if (answer != null) await CommandResponseHelper.ExecuteAsync(_serviceScopeFactory, request.Client!,
                (long)(request.ChatId!),
                answer);
        }
    }
}
