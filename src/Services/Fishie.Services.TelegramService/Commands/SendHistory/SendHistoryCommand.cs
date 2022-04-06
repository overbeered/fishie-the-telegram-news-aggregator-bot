using MediatR;
using WTelegram;

namespace Fishie.Services.TelegramService.Commands
{
    /// <summary>
    /// Get the message history from the channel.  Example: /sendMessagesHistory chat name | 5
    /// </summary>
    internal class SendHistoryCommand : IRequest
    {
        public Client? Client { get; set; }
        public long? ChatId { get; set; }
        public string? Action { get; set; }
    }
}
