using Fishie.Core.Repositories;
using Fishie.Services.TelegramService.Commands.Utils;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using TL;
using WTelegram;
using CoreModels = Fishie.Core.Models;

namespace Fishie.Services.TelegramService.Commands.AddChat;

/// <summary>
/// Find and add a chat to the database. Example: /addChat chat name
/// </summary>
internal class AddChatCommandHandler : AsyncRequestHandler<AddChatCommand>, IDisposable
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IDisposableResource _disposableResource;
    private readonly Client _client;

    public AddChatCommandHandler(IServiceScopeFactory serviceScopeFactory,
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

    protected override async Task Handle(AddChatCommand request, CancellationToken cancellationToken)
    {
        string? answer = null;

        if (request.Action!.IndexOf("--info") != -1)
        {
            answer = "Find and add a chat to the database. Example: /addChat chat name";
        }
        else
        {
            var search = await _client.Contacts_Search(request.Action);

            if (search.chats.Count == 0)
            {
                answer = $"chat {request.Action} not found";
            }
            else
            {
                foreach (var (id, chat) in search.chats)
                {
                    if (chat.Title == request.Action)
                    {
                        var channel = (InputPeerChannel)chat.ToInputPeer();
                        var coreChat = new CoreModels.Chat(
                                channel.channel_id,
                                channel.access_hash,
                                chat.Title,
                                ((Channel)chat).username);

                        using var scope = _serviceScopeFactory.CreateScope();
                        var chatRepository = scope.ServiceProvider.GetRequiredService<IChatRepository>();

                        answer = $"The chat {request.Action} has already been added to the database";

                        if (!await chatRepository.ExistsAsync(coreChat))
                        {
                            await chatRepository!.AddAsync(coreChat);
                            answer = $"The chat {request.Action} has been added to the database";
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