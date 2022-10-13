using Fishie.Core.Models;
using Fishie.Core.Repositories;
using Fishie.Services.TelegramService.Commands.Utils;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using WTelegram;

namespace Fishie.Services.TelegramService.Commands.Forward;

/// <summary>
/// Subscribe to message forward. Example: /forward channel name
/// </summary>
internal class ForwardCommandHandler : AsyncRequestHandler<ForwardCommand>, IDisposable
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IDisposableResource _disposableResource;
    private readonly Client _client;

    public ForwardCommandHandler(IServiceScopeFactory serviceScopeFactory,
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

    protected override async Task Handle(ForwardCommand request, CancellationToken cancellationToken)
    {
        string? answer = null;

        if (request.Action!.IndexOf("--info") != -1)
        {
            answer = "Subscribe to message forward. Example: /forward channel username";
        }
        else
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var channelRepository = scope.ServiceProvider.GetRequiredService<IChannelRepository>();
            var forwardMessagesRepository = scope.ServiceProvider.GetRequiredService<IForwardMessagesRepository>();

            var channel = await channelRepository!.FindAsync(request.Action);

            answer = $"Channel {request.Action} not found in the database";

            if (channel != null)
            {
                var forwardMessages = new ForwardMessages(channel.Id, (long)request.ChatId!);

                if (!await forwardMessagesRepository.ExistsAsync(forwardMessages))
                {
                    await forwardMessagesRepository.AddAsync(forwardMessages);
                    answer = $"You are subscribed to channel updates {request.Action}";
                }
                else
                {
                    answer = $"You have already subscribed to message updates for this channel {request.Action}";
                }
            }
        }

        if (answer != null) await CommandResponseHelper.ExecuteAsync(_serviceScopeFactory,
            _client,
            request.ChatId!.Value,
            answer);
    }
}
