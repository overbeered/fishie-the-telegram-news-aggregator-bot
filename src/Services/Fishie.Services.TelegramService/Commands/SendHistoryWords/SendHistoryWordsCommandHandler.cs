using Fishie.Core.Repositories;
using Fishie.Services.TelegramService.Commands.Utils;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using TL;
using WTelegram;

namespace Fishie.Services.TelegramService.Commands.SendHistoryWords;

/// <summary>
/// Get the message history from the channel by word. Example: /sendHistoryWords channel username | 5 | words
/// </summary>
internal class SendHistoryWordsCommandHandler : AsyncRequestHandler<SendHistoryWordsCommand>, IDisposable
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IDisposableResource _disposableResource;
    private readonly Client _client;

    public SendHistoryWordsCommandHandler(IServiceScopeFactory serviceScopeFactory,
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

    protected override async Task Handle(SendHistoryWordsCommand request, CancellationToken cancellationToken)
    {
        string? answer = null;

        if (request.Action!.IndexOf("--info") != -1)
        {
            answer = "Get the message history from the channel by word. Example: /sendHistoryWords channel username | 5 | words";
        }
        else
        {
            var channelUsername = request.Action.Remove(request.Action.IndexOf("|") - 1);
            request.Action = request.Action.Remove(0, request.Action.IndexOf("|") + 2);
            var count = int.Parse(request.Action.Remove(request.Action.IndexOf("|") - 1));
            request.Action = request.Action.Remove(0, request.Action.IndexOf("|") + 2);

            using var scope = _serviceScopeFactory.CreateScope();
            var channelRepository = scope.ServiceProvider.GetRequiredService<IChannelRepository>();
            var chatRepository = scope.ServiceProvider.GetRequiredService<IChatRepository>();

            var channel = await channelRepository.FindAsync(channelUsername);
            var chat = await chatRepository.FindAsync(request.ChatId!.Value);

            if (channel == null)
            {
                answer = $"Channels {channelUsername} not found in the database";
            }
            else
            {
                var messagesIdList = new List<int>();

                var messages = await _client.Messages_GetHistory(new InputChannel(channel.Id, channel.AccessHash));

                for (int msgNumber = 0; msgNumber < count; msgNumber++)
                {
                    var message = (Message)messages.Messages[msgNumber];

                    if (message.message.IndexOf(request.Action) != -1) messagesIdList.Add(message.ID);
                }

                foreach (var idMessage in messagesIdList)
                {
                    await _client.Messages_ForwardMessages(new InputChannel(channel.Id, channel.AccessHash),
                    new int[] { idMessage },
                    new long[] { Random.Shared.Next(int.MinValue, int.MaxValue) },
                    new InputChannel(chat!.Id, chat.AccessHash));
                }
            }
        }

        if (answer != null) await CommandResponseHelper.ExecuteAsync(_serviceScopeFactory,
            _client,
            request.ChatId!.Value,
            answer);
    }
}