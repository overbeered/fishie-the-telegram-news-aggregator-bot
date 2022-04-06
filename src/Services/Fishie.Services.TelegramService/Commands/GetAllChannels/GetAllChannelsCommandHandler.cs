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
    internal class GetAllChannelsCommandHandler : AsyncRequestHandler<DeleteChannelCommand>
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public GetAllChannelsCommandHandler(IServiceScopeFactory serviceScopeFactory)
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
                        "Sends a list of channels to the chat. Example: /getAllChannels");
                }
            }
            else
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    IChannelRepository channalRepository = scope.ServiceProvider.GetRequiredService<IChannelRepository>();
                    var listChannels = await channalRepository.GetAllChannelsAsync();
                    string message = "";

                    if (listChannels!.Count() != 0)
                    {
                        foreach (var channel in listChannels!)
                        {
                            message += "Id: " + channel!.Id + " Name: " + channel!.Name + " AccessHash: " + channel.AccessHash + "\n";
                        }

                        await ResponseCommand.ExecuteAsync(_serviceScopeFactory,
                            request.Client!,
                            (long)request.ChatId!,
                            message);
                    }
                    else
                    {
                        await ResponseCommand.ExecuteAsync(_serviceScopeFactory,
                            request.Client!,
                            (long)request.ChatId!,
                            "Channels not found");
                    }
                }
            }
        }
    }
}
