using MediatR;

namespace Fishie.Services.TelegramService.Commands;

/// <summary>
/// List of subscribed channels for sending new messages to the chat. Example: /getAllForwardCommandHandler
/// </summary>
internal class GetAllForwardCommand : Command, IRequest
{
    public static readonly string CommandText = "getAllForward";
}