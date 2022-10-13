using Fishie.Core.Repositories;
using Fishie.Services.TelegramService.Commands;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using TL;
using WTelegram;

namespace Fishie.Services.TelegramService;

/// <summary>
/// Message Handler
/// </summary>
internal class MessagesHandler : AsyncRequestHandler<MessagesRequest>, IDisposable
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IMediator _mediator;
    private readonly Client _client;
    private readonly Dictionary<string, Command> _handlers;
    private readonly IDisposableResource _disposableResource;

    public MessagesHandler(IMediator mediator, 
        IServiceScopeFactory serviceScopeFactory, 
        Client client, 
        IDisposableResource disposableResource)
    {
        _mediator = mediator;
        _serviceScopeFactory = serviceScopeFactory;
        _client = client;
        _disposableResource = disposableResource;
        _handlers = new Dictionary<string, Command>()
            {
                {AddAdminCommand.CommandText, new AddAdminCommand()},
                {AddChannelCommand.CommandText, new AddChannelCommand()},
                {AddChatCommand.CommandText, new AddChatCommand()},
                {DeleteChannelCommand.CommandText, new DeleteChannelCommand()},
                {GetAllChannelsCommand.CommandText, new GetAllChannelsCommand()},
                {SubscribeCommand.CommandText, new SubscribeCommand()},
                {UnsubscribeCommand.CommandText, new UnsubscribeCommand()},
                {SendHistoryCommand.CommandText, new SendHistoryCommand()},
                {SendHistoryWordsCommand.CommandText, new SendHistoryWordsCommand()},
                {ForwardCommand.CommandText, new ForwardCommand()},
                {GetAllForwardCommand.CommandText, new GetAllForwardCommand()},
                {DeleteChannelForwardCommand.CommandText, new DeleteChannelForwardCommand()},
            };
    }

    protected override async Task Handle(MessagesRequest request, CancellationToken cancellationToken)
    {
        try
        {
            if (request.Message!.IndexOf("/") == 0)
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var adminRepository = scope.ServiceProvider.GetRequiredService<IAdminRepository>();

                if (await adminRepository.ExistsAsync(request.UserId!.Value))
                {
                    string message = request.Message!.Remove(0, 1);
                    string command = message.IndexOf(" ") != -1 ? message.Remove(message.IndexOf(" ")) : message;
                    string? action = command != message ? message.Remove(0, message.IndexOf(" ") + 1) : null;

                    if (command == "commands")
                    {
                        action = " ";

                        foreach (var key in _handlers.Keys)
                        {
                            action += "\n" + key;
                        }

                        await SendMessageAsync(request.ChatId!.Value, action);
                    }
                    else
                    {
                        _handlers[command].ChatId = request.ChatId;
                        _handlers[command].Action = action;

                        await _mediator.Send(_handlers[command], cancellationToken);
                    }
                }
            }
            else
            {
                await ForwardMessagesAsync(request.ChatId!.Value, (int)request.MessageId!.Value);
            }
        }
        catch (KeyNotFoundException)
        {
            await SendMessageAsync(request.ChatId!.Value, $"Сommand \"{request.Message}\" not found");
        }
    }

    /// <summary>
    /// Sends a message to the chat
    /// </summary>
    /// <param name="chatId">Chat id</param>
    /// <param name="message">Message</param>
    private async Task SendMessageAsync(long chatId, string message)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var chatRepository = scope.ServiceProvider.GetRequiredService<IChatRepository>();

        var chat = await chatRepository.FindAsync(chatId);

        await _client.SendMessageAsync(new InputChannel(chat!.Id, chat.AccessHash), message);
    }

    /// <summary>
    /// Forward messages to chat
    /// </summary>
    /// <param name="channelId">Сhannel\chat id</param>
    /// <param name="messageId">Message id</param>
    private async Task ForwardMessagesAsync(long channelId, int messageId)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var forwardMessagesRepository = scope.ServiceProvider.GetRequiredService<IForwardMessagesRepository>();

        if (await forwardMessagesRepository.ChannelIdExistsAsync(channelId))
        {
            var chatRepository = scope.ServiceProvider.GetRequiredService<IChatRepository>();
            var channelRepository = scope.ServiceProvider.GetRequiredService<IChannelRepository>();

            var forwardMessagesList = await forwardMessagesRepository.FindChannelIdAsync(channelId);
            var channel = await channelRepository.FindAsync(channelId);

            foreach (var forwardMessages in forwardMessagesList)
            {
                var chat = await chatRepository.FindAsync(forwardMessages!.ChatId);

                await _client.Messages_ForwardMessages(
                    new InputChannel(channel!.Id, channel.AccessHash),
                    new int[] { messageId },
                    new long[] { Random.Shared.Next(int.MinValue, int.MaxValue) },
                    new InputChannel(chat!.Id, chat.AccessHash));
            }
        }
    }

    public void Dispose()
    {
        _disposableResource.Dispose();
    }
}