using Fishie.Core.Repositories;
using Fishie.Services.TelegramService.Commands.Utils;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Fishie.Services.TelegramService.Commands.GetAllForward
{
    /// <summary>
    /// List of subscribed channels for sending new messages to the chat. Example: /getAllForward
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
            string? answer = null;

            if (request.Action != null && request.Action.IndexOf("--info") != -1)
            {
                answer = "List of subscribed channels for sending new messages to the chat. Example: /getAllForward";
            }
            else
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    IForwardMessagesRepository forwardMessagesRepository = scope.ServiceProvider
                        .GetRequiredService<IForwardMessagesRepository>();
                    var listSendMessages = await forwardMessagesRepository.FindAllForwardMessagesAsync();
                    answer = "No tracking for this chat";

                    if (listSendMessages != null)
                    {
                        listSendMessages = listSendMessages.Where(c => c!.ChatId == request.ChatId).ToList();

                        if (listSendMessages.Count() != 0)
                        {
                            answer = " ";
                            IChannelRepository channalRepository = scope.ServiceProvider.GetRequiredService<IChannelRepository>();

                            foreach (var list in listSendMessages)
                            {
                                var channel = await channalRepository.FindChannelByIdAsync(list!.ChannelId);
                                answer += "Name: " + channel!.Name + "; Username: " + channel!.Username + "\n";
                            }
                        }
                    }
                }
            }

            if (answer != null) await CommandResponseHelper.ExecuteAsync(_serviceScopeFactory, request.Client!,
                (long)(request.ChatId!),
                answer);
        }
    }
}
