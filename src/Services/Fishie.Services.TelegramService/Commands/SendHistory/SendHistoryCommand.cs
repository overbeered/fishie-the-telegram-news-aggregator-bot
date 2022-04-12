using MediatR;

namespace Fishie.Services.TelegramService.Commands
{
    /// <summary>
    /// Get the message history from the channel.  Example: /sendMessagesHistory chat name | 5
    /// </summary>
    internal class SendHistoryCommand : Command, IRequest
    {
        public static readonly string CommandText = "sendHistory";
    }
}
