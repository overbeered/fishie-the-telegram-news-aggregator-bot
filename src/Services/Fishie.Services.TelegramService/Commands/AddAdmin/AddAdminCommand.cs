using MediatR;

namespace Fishie.Services.TelegramService.Commands
{
    /// <summary>
    /// Find and add a admin to the database. Example: /addAdmin username
    /// </summary>
    internal class AddAdminCommand : Command, IRequest
    {
        public static readonly string CommandText = "addAdmin";
    }
}
