using Fishie.Core.Repositories;
using Fishie.Services.TelegramService.Commands.Utils;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using WTelegram;

namespace Fishie.Services.TelegramService.Commands.DeleteChannel;

/// <summary>
/// Delete from the database channel\chat. Example: /deleteChannel channel username
/// </summary>
internal class DeleteChannelCommandHandler : AsyncRequestHandler<DeleteChannelCommand>, IDisposable
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IDisposableResource _disposableResource;
    private readonly Client _client;

    public DeleteChannelCommandHandler(IServiceScopeFactory serviceScopeFactory,
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

    protected override async Task Handle(DeleteChannelCommand request, CancellationToken cancellationToken)
    {
        string? answer = null;

        if (request.Action!.IndexOf("--info") != -1)
        {
            answer = "Delete from the database channel\\chat. Example: /deleteChannel channel username";
        }
        else
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var channelRepository = scope.ServiceProvider.GetRequiredService<IChannelRepository>();

            var channel = await channelRepository.FindAsync(request.Action);

            answer = $"The channel {request.Action} is not stored in the database";

            if (channel != null)
            {
                await channelRepository.DeleteAsync(request.Action);

                answer = $"The channel {request.Action} has been deleted";
            }
        }

        if (answer != null) await CommandResponseHelper.ExecuteAsync(_serviceScopeFactory,
            _client!,
            request.ChatId!.Value,
            answer);
    }
}