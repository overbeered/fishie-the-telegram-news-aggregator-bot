using MediatR;

namespace Fishie.Services.TelegramService.Commands
{
    /// <summary>
    /// Delete from the database channel\chat. Example: /deleteChannel channel name
    /// </summary>
    internal class DeleteChannelCommand : Command, IRequest
    {
        public static readonly string CommandText = "deleteChannel";
    }
}
