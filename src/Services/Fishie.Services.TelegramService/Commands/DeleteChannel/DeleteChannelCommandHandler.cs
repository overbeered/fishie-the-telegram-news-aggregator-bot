using Fishie.Core.Repositories;
using Fishie.Services.TelegramService.Commands.Utils;
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
            string? answer = null;

            if (request.Action!.IndexOf("--info") != -1)
            {
                answer = "Delete from the database channel\\chat. Example: /deleteChannel channel name";
            }
            else
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    IChannelRepository channelRepository = scope.ServiceProvider.GetRequiredService<IChannelRepository>();
                    IForwardMessagesRepository forwardMessagesRepository = scope.ServiceProvider.GetRequiredService<IForwardMessagesRepository>();
                    var channel = await channelRepository.FindChannelAsync(request.Action);

                    answer = $"The channel {request.Action} is not stored in the database";

                    if (channel != null)
                    {
                        await channelRepository.DeleteChannelAsync(request.Action);
                        await forwardMessagesRepository.DeleteForwardChannelByIdAsync(channel.Id);

                        answer = $"The channel {request.Action} has been deleted";
                    }
                }
            }

            if (answer != null) await CommandResponseHelper.ExecuteAsync(_serviceScopeFactory, request.Client!,
                (long)(request.ChatId!),
                answer);
        }
    }
}
