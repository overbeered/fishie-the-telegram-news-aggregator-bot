using MediatR;

namespace Fishie.Services.TelegramService.Commands
{
    /// <summary>
    /// Remove channel tracking. Example: /deleteChannelForward channel name
    /// </summary>
    internal class DeleteChannelForwardCommand : Command, IRequest
    {
        public static readonly string CommandText = "deleteChannelForward";
    }
}
