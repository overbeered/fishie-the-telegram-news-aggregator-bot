using MediatR;

namespace Fishie.Services.TelegramService.Commands
{
    /// <summary>
    /// Send a message to the chat where the command was called
    /// </summary>
    internal class SendCommand : Command, IRequest
    {
        public static readonly string CommandText = "commands";
    }
}
