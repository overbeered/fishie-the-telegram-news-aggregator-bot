using MediatR;
using WTelegram;

namespace Fishie.Services.TelegramService.Commands
{
    /// <summary>
    /// Find and add a channel\chat to the database. Example: /addChannel channel name
    /// </summary>
    internal class AddChannelCommand : IRequest
    {
        public Client? Client { get; set; }
        public long? ChatId { get; set; }
        public string? Action { get; set; }
    }
}
