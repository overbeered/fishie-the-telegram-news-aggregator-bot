using Fishie.Core.Repositories;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Fishie.Services.TelegramService.Commands.GetAllForward
{
    /// <summary>
    /// List of subscribed channels for sending new messages to the chat. Example: /getAllForwardCommandHandler
    /// </summary>
    internal class GetAllForwardCommandHandler : AsyncRequestHandler<GetAllForwardCommand>
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public GetAllForwardCommandHandler(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task Handle(GetAllForwardCommand request, CancellationToken cancellationToken)
        {
            if (request.Action != null && request.Action.IndexOf("--info") != -1)
            {
                await ResponseCommand.ExecuteAsync(_serviceScopeFactory,
                    request.Client!,
                    (long)request.ChatId!,
                    "List of subscribed channels for sending new messages to the chat. Example: /getAllForwardCommandHandler");
            }
            else
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    IForwardMessagesRepository forwardMessagesRepository = scope.ServiceProvider
                        .GetRequiredService<IForwardMessagesRepository>();
                    var listSendMessages = await forwardMessagesRepository.GetAllForwardMessagesAsync();
                    string message = "No tracking for this chat";

                    if (listSendMessages != null)
                    {
                        listSendMessages = listSendMessages!.Where(c => c!.ChatId == request.ChatId);

                        if (listSendMessages.Count() != 0)
                        {
                            message = "";
                            IChannelRepository channalRepository = scope.ServiceProvider.GetRequiredService<IChannelRepository>();

                            foreach (var list in listSendMessages)
                            {
                                var channel = await channalRepository.GetChannelByIdAsync(list!.ChannelId);
                                message += "Id: " + channel!.Id + " Name: " + channel!.Name + " AccessHash: " + channel.AccessHash + "\n";

                            }
                        }
                    }


                    await ResponseCommand.ExecuteAsync(_serviceScopeFactory,
                        request.Client!,
                        (long)request.ChatId!,
                        message);
                }
            }
        }
    }
}
