using Fishie.Core.Repositories;
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
            if (request.Action != null && request.Action.IndexOf("--info") != -1)
            {
                await ResponseCommand.ExecuteAsync(_serviceScopeFactory,
                    request.Client!,
                    (long)request.ChatId!,
                    "Sends a list of channels to the chat. Example: /getAllChannels");
            }
            else
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    IChannelRepository channalRepository = scope.ServiceProvider.GetRequiredService<IChannelRepository>();
                    var listChannels = await channalRepository.GetAllChannelsAsync();
                    string message = "Channels not found";

                    if (listChannels!.Count() != 0)
                    {
                        message = "";
                        foreach (var channel in listChannels!)
                        {
                            message += "Id: " + channel!.Id + " Name: " + channel!.Name + " AccessHash: " + channel.AccessHash + "\n";
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
