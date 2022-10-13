using MediatR;

namespace Fishie.Services.TelegramService.Commands;

/// <summary>
/// Find and add a chat to the database. Example: /addChat chat name
/// </summary>
internal class AddChatCommand : Command, IRequest
{
    public static readonly string CommandText = "addChat";
}