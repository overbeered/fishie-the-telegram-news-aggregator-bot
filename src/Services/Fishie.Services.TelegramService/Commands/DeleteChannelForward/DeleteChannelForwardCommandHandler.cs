using Fishie.Core.Repositories;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Fishie.Services.TelegramService.Commands.DeleteChannelForward
{
    /// <summary>
    /// Remove channel tracking. Example: /deleteChannelForward channel name
    /// </summary>
    internal class DeleteChannelForwardCommandHandler : AsyncRequestHandler<DeleteChannelForwardCommand>
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public DeleteChannelForwardCommandHandler(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task Handle(DeleteChannelForwardCommand request, CancellationToken cancellationToken)
        {
            if (request.Action!.IndexOf("--info") != -1)
            {
                await ResponseCommand.ExecuteAsync(_serviceScopeFactory,
                    request.Client!,
                    (long)request.ChatId!,
                    "Remove channel tracking. Example: /deleteChannelForward channel name");
            }
            else
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    IChannelRepository channelRepository = scope.ServiceProvider.GetRequiredService<IChannelRepository>();
                    IForwardMessagesRepository forwardMessagesRepository = scope.ServiceProvider.GetRequiredService<IForwardMessagesRepository>();
                    var channel = await channelRepository.GetChannelAsync(request.Action);
                    if (channel != null)
                    {
                        var listSendMessages = await forwardMessagesRepository.GetAllForwardMessagesAsync();
                        var forwardMessages = listSendMessages!.FirstOrDefault(c =>
                        c!.ChatId == request.ChatId && c.ChannelId == channel.Id);
                        if (forwardMessages != null) await forwardMessagesRepository.DeleteForwardMessagesAsync(forwardMessages);
                    }

                }
            }
        }
    }
}
