using Fishie.Core.Repositories;
using Fishie.Services.TelegramService.Commands.Utils;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using TL;
using WTelegram;
using CoreModels = Fishie.Core.Models;

namespace Fishie.Services.TelegramService.Commands.AddChannel;

/// <summary>
/// Find and add a channel\chat to the database. Example: /addChannel channel username
/// </summary>
internal class AddChannelCommandHandler : AsyncRequestHandler<AddChannelCommand>, IDisposable
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IDisposableResource _disposableResource;
    private readonly Client _client;

    public AddChannelCommandHandler(IServiceScopeFactory serviceScopeFactory,
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

    protected override async Task Handle(AddChannelCommand request, CancellationToken cancellationToken)
    {
        string? answer = null;

        if (request.Action!.IndexOf("--info") != -1)
        {
            answer = "Find and add a channel\\chat to the database. Example: /addChannel channel username";
        }
        else
        {
            var search = await _client.Contacts_Search(request.Action);

            if (search.chats.Count == 0)
            {
                answer = $"channel {request.Action} not found";
            }
            else
            {
                foreach (var (_, chat) in search.chats)
                {
                    if (((Channel)chat).username == request.Action)
                    {
                        var channel = (InputPeerChannel)chat.ToInputPeer();
                        var coreChannel = new CoreModels.Channel(
                                channel.channel_id,
                                channel.access_hash,
                                chat.Title,
                                ((Channel)chat).username);

                        using var scope = _serviceScopeFactory.CreateScope();
                        var channelRepository = scope.ServiceProvider.GetRequiredService<IChannelRepository>();

                        answer = $"The channel {request.Action} has already been added to the database";

                        if (!await channelRepository.ExistsAsync(coreChannel))
                        {
                            await channelRepository!.AddAsync(coreChannel);
                            answer = $"The channel {request.Action} has been added to the database";
                        }
                        break;
                    }
                }
            }
        }

        if (answer != null) await CommandResponseHelper.ExecuteAsync(_serviceScopeFactory,
            _client,
            request.ChatId!.Value,
            answer);
    }
}