using Fishie.Core.Repositories;
using Fishie.Services.TelegramService.Commands.Utils;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Fishie.Services.TelegramService.Commands.GetAllChannels
{
    /// <summary>
    /// Sends a list of channels to the chat. Example: /getAllChannels
    /// </summary>
    internal class GetAllChannelsCommandHandler : AsyncRequestHandler<GetAllChannelsCommand>
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public GetAllChannelsCommandHandler(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task Handle(GetAllChannelsCommand request, CancellationToken cancellationToken)
        {
            string? answer = null;

            if (request.Action != null && request.Action.IndexOf("--info") != -1)
            {
                answer = "Sends a list of channels to the chat. Example: /getAllChannels";
            }
            else
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    IChannelRepository channalRepository = scope.ServiceProvider.GetRequiredService<IChannelRepository>();
                    var listChannels = await channalRepository.FindAllChannelsAsync();
                    answer = "Channels not found";

                    if (listChannels!.Count() != 0)
                    {
                        answer = "";
                        foreach (var channel in listChannels!)
                        {
                            answer += "Name: " + channel!.Name + "; Username: " + channel!.Username + "\n";
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
