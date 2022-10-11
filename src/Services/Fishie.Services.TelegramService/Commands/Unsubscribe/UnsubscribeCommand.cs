using MediatR;

namespace Fishie.Services.TelegramService.Commands;

/// <summary>
/// Unsubscribe to the channel from the database. Example: /unsubscribe channel name
/// </summary>
internal class UnsubscribeCommand : Command, IRequest
{
    public static readonly string CommandText = "unsubscribe";
}