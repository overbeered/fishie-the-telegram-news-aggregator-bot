using Fishie.Core.Services;
using Fishie.Services.TelegramService.Configuration;
using MediatR;
using Microsoft.Extensions.Logging;
using TL;
using WTelegram;

namespace Fishie.Services.TelegramService;

public class TelegramServices : ITelegramServices
{
    public bool Disconnected { get { return _client.Disconnected; } }

    private readonly ILogger<TelegramServices> _logger;
    private readonly Client _client;
    private readonly IMediator _mediator;

    public TelegramServices(ILogger<TelegramServices> logger,
        Client client,
        IMediator mediator)
    {
        _logger = logger;
        _client = client;
        _mediator = mediator;
        _client.OnUpdate += OnUpdates;
    }

    public async Task LoginAsync()
    {
        await _client.LoginUserIfNeeded();
        await _mediator.Publish(new ConfigurationNotification());
    }

    public void Reset()
    {
        _client.Reset();
    }

    /// <summary>
    /// Updating events
    /// </summary>
    private async Task OnUpdates(IObject obj)
    {
        if (obj is not UpdatesBase updates) return;
        foreach (var update in updates.UpdateList)
            switch (update)
            {
                case UpdateNewMessage unm: await DisplayMessageAsync(unm.message); break;
            }
    }

    /// <summary>
    /// Processes messages
    /// </summary>
    /// <param name="messageBase">Describing a message</param>
    private async Task DisplayMessageAsync(MessageBase messageBase)
    {
        try
        {
            switch (messageBase)
            {
                case Message m:
                    await _mediator.Send(new MessagesRequest()
                    {
                        UserId = m.From != null ? m.From.ID : null,
                        ChatId = m.Peer.ID,
                        MessageId = m.ID,
                        Message = m.message,
                    });
                    break;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in Services: {ServicesName} in Method: {MethodName},",
                nameof(TelegramServices),
                nameof(DisplayMessageAsync));
        }
    }
}