using Fishie.Core.Repositories;
using Fishie.Services.TelegramService.Commands.Utils;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using WTelegram;

namespace Fishie.Services.TelegramService.Commands.GetAllChannels;

/// <summary>
/// Sends a list of channels to the chat. Example: /getAllChannels
/// </summary>
internal class GetAllChannelsCommandHandler : AsyncRequestHandler<GetAllChannelsCommand>, IDisposable
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IDisposableResource _disposableResource;
    private readonly Client _client;

    public GetAllChannelsCommandHandler(IServiceScopeFactory serviceScopeFactory, IDisposableResource disposableResource, Client client)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _disposableResource = disposableResource;
        _client = client;
    }

    public void Dispose()
    {
        _disposableResource?.Dispose();
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
            using var scope = _serviceScopeFactory.CreateScope();
            var channalRepository = scope.ServiceProvider.GetRequiredService<IChannelRepository>();

            var listChannels = await channalRepository.FindAllAsync();

            answer = "Channels not found";

            if (listChannels.Count != 0)
            {
                answer = "";
                foreach (var channel in listChannels!)
                {
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