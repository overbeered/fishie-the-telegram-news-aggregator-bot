using MediatR;

namespace Fishie.Services.TelegramService;

internal class MessagesRequest : IRequest
{
    public long? UserId { get; set; }
    public long? ChatId { get; set; }
    public long? MessageId { get; set; }
    public string? Message { get; set; }
}