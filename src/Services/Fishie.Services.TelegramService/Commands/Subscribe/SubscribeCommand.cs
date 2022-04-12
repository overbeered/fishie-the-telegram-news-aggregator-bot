using MediatR;

namespace Fishie.Services.TelegramService.Commands
{
    /// <summary>
    /// Subscribe to the channel from the database. Example: /subscribe channel name
    /// </summary>
    internal class SubscribeCommand : Command, IRequest
    {
        public static readonly string CommandText = "subscribe";
    }
}
