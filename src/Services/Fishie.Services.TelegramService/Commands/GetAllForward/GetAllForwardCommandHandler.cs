using Fishie.Core.Repositories;
using Fishie.Services.TelegramService.Commands.Utils;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using WTelegram;

namespace Fishie.Services.TelegramService.Commands.GetAllForward;

/// <summary>
/// List of subscribed channels for sending new messages to the chat. Example: /getAllForward
/// </summary>
internal class GetAllForwardCommandHandler : AsyncRequestHandler<GetAllForwardCommand>, IDisposable
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IDisposableResource _disposableResource;
    private readonly Client _client;

    public GetAllForwardCommandHandler(IServiceScopeFactory serviceScopeFactory,
        IDisposableResource disposableResource,
        Client client)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _disposableResource = disposableResource;
        _client = client;
    }

    public void Dispose()
    {
        _disposableResource?.Dispose();
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
            using var scope = _serviceScopeFactory.CreateScope();
            var forwardMessagesRepository = scope.ServiceProvider.GetRequiredService<IForwardMessagesRepository>();
            var channalRepository = scope.ServiceProvider.GetRequiredService<IChannelRepository>();

            var listSendMessages = await forwardMessagesRepository.FindChatIdAsync(request.ChatId!.Value);

            answer = "No tracking for this chat";

            if (listSendMessages.Count != 0)
            {
                answer = " ";

                foreach (var list in listSendMessages)
                {
                    var channel = await channalRepository.FindAsync(list!.ChannelId);
                    answer += "Name: " + channel!.Name + "; Username: " + channel!.Username + "\n";
                }
            }

        }

        if (answer != null) await CommandResponseHelper.ExecuteAsync(_serviceScopeFactory,
            _client,
            request.ChatId!.Value,
            answer);
    }
}