using MediatR;
using WTelegram;

namespace Fishie.Services.TelegramService.Commands
{
    /// <summary>
    /// Get the message history from the channel by word. Example: /sendHistoryWords chat name | 5 | words
    /// </summary>
    internal class SendHistoryWordsCommand : IRequest
    {
        public Client? Client { get; set; }
        public long? ChatId { get; set; }
        public string? Action { get; set; }
    }
}
