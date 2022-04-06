using MediatR;
using WTelegram;

namespace Fishie.Services.TelegramService.Commands
{
    /// <summary>
    /// Subscribe to the channel from the database. Example: /subscribe channel name
    /// </summary>
    internal class SubscribeCommand : IRequest
    {
        public Client? Client { get; set; }
        public long? ChatId { get; set; }
        public string? Action { get; set; }
    }
}
