using MediatR;

namespace Fishie.Services.TelegramService.Commands
{
    /// <summary>
    /// Subscribe to message forward. Example: /forward channel name
    /// </summary>
    internal class ForwardCommand : Command, IRequest
    {
        public static readonly string CommandText = "forward";
    }
}
