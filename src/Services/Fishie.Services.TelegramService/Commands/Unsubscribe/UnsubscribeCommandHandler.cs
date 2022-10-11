using Fishie.Core.Repositories;
using Fishie.Services.TelegramService.Commands.Utils;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using TL;
using WTelegram;

namespace Fishie.Services.TelegramService.Commands.Unsubscribe;

/// <summary>
/// Unsubscribe to the channel from the database. Example: /unsubscribe channel username
/// </summary>
internal class UnsubscribeCommandHandler : AsyncRequestHandler<UnsubscribeCommand>, IDisposable
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IDisposableResource _disposableResource;
    private readonly Client _client;

    public UnsubscribeCommandHandler(IServiceScopeFactory serviceScopeFactory,
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

    protected override async Task Handle(UnsubscribeCommand request, CancellationToken cancellationToken)
    {
        string? answer = null;

        if (request.Action!.IndexOf("--info") != -1)
        {
            answer = "Unsubscribe to the channel from the database. Example: /unsubscribe channel username";
        }
        else
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var chatRepository = scope.ServiceProvider.GetRequiredService<IChannelRepository>();

            var channel = await chatRepository.FindAsync(request.Action);

            answer = $"Channels {request.Action} not found in the database";

            if (channel != null)
            {
                await _client.LeaveChat(new InputChannel(channel.Id, channel.AccessHash));
                answer = $"You have unsubscribed to the channel {request.Action}";
            }
        }

        if (answer != null) await CommandResponseHelper.ExecuteAsync(_serviceScopeFactory,
            _client,
            request.ChatId!.Value,
            answer);
    }
}