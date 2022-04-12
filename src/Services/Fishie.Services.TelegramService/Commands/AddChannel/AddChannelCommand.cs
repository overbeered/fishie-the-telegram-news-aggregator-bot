using MediatR;

namespace Fishie.Services.TelegramService.Commands
{
    /// <summary>
    /// Find and add a channel\chat to the database. Example: /addChannel channel name
    /// </summary>
    internal class AddChannelCommand : Command, IRequest
    {
        public static readonly string CommandText = "addChannel";
    }
}

