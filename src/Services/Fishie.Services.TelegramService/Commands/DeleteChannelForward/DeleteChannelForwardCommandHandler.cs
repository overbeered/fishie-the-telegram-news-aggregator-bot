using Fishie.Core.Models;
using Fishie.Core.Repositories;
using Fishie.Services.TelegramService.Commands.Utils;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using WTelegram;

namespace Fishie.Services.TelegramService.Commands.DeleteChannelForward;

/// <summary>
/// Remove channel tracking. Example: /deleteChannelForward channel username
/// </summary>
internal class DeleteChannelForwardCommandHandler : AsyncRequestHandler<DeleteChannelForwardCommand>, IDisposable
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IDisposableResource _disposableResource;
    private readonly Client _client;

    public DeleteChannelForwardCommandHandler(IServiceScopeFactory serviceScopeFactory,
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

    protected override async Task Handle(DeleteChannelForwardCommand request, CancellationToken cancellationToken)
    {
        string? answer = null;

        if (request.Action!.IndexOf("--info") != -1)
        {
            answer = "Remove channel tracking. Example: /deleteChannelForward channel username";
        }
        else
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var channelRepository = scope.ServiceProvider.GetRequiredService<IChannelRepository>();
            var forwardMessagesRepository = scope.ServiceProvider.GetRequiredService<IForwardMessagesRepository>();

            var channel = await channelRepository.FindAsync(request.Action);

            answer = $"The channel {request.Action} not stored in the surveillance database";

            if (channel != null)
            {
                await forwardMessagesRepository.RemoveAsync(new ForwardMessages(channel.Id, request.ChatId!.Value));

                answer = $"The channel {request.Action} was removed from tracking";
            }
        }

        if (answer != null) await CommandResponseHelper.ExecuteAsync(_serviceScopeFactory,
            _client,
            request.ChatId!.Value,
            answer);
    }
}