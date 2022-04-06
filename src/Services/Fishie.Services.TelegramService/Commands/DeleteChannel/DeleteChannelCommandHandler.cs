using Fishie.Core.Repositories;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using System.Threading.Tasks;

namespace Fishie.Services.TelegramService.Commands.DeleteChannel
{
    /// <summary>
    /// Delete from the database channel\chat. Example: /deleteChannel channel name
    /// </summary>
    internal class DeleteChannelCommandHandler : AsyncRequestHandler<DeleteChannelCommand>
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public DeleteChannelCommandHandler(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task Handle(DeleteChannelCommand request, CancellationToken cancellationToken)
        {
            if (request.Action!.IndexOf("--info") != -1)
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    await ResponseCommand.ExecuteAsync(_serviceScopeFactory,
                        request.Client!,
                        (long)request.ChatId!,
                        "Delete from the database channel\\chat. Example: /deleteChannel channel name");
                }
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
                        await channelRepository.DeleteChannelAsync(request.Action);
                        await forwardMessagesRepository.DeleteForwardChannelByIdAsync(channel.Id);
                    }
                }
            }
        }
    }
}
