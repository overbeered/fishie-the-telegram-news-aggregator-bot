using MediatR;

namespace Fishie.Services.TelegramService.Commands;

/// <summary>
/// Sends a list of channels to the chat. Example: /getAllChannels
/// </summary>
internal class GetAllChannelsCommand : Command, IRequest
{
    public static readonly string CommandText = "getAllChannels";
}