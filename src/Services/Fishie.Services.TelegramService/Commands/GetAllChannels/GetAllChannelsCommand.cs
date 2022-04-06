using MediatR;
using WTelegram;

namespace Fishie.Services.TelegramService.Commands
{
    /// <summary>
    /// Sends a list of channels to the chat. Example: /getAllChannels
    /// </summary>
    internal class GetAllChannelsCommand : IRequest
    {
        public Client? Client { get; set; }
        public long? ChatId { get; set; }
        public string? Action { get; set; }
    }
}
