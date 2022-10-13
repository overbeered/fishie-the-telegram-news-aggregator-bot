using MediatR;

namespace Fishie.Services.TelegramService;

/// <summary>
/// Request for messages
/// </summary>
internal class MessagesRequest : IRequest
{
    /// <summary>
    /// Telegram user Id
    /// </summary>
    public long? UserId { get; set; }

    /// <summary>
    /// Telegram chat Id
    /// </summary>
    public long? ChatId { get; set; }

    /// <summary>
    /// Message Id
    /// </summary>
    public long? MessageId { get; set; }

    /// <summary>
    /// Message
    /// </summary>
    public string? Message { get; set; }
}