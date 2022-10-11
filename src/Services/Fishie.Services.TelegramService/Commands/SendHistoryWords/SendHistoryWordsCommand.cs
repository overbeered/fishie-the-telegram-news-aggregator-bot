using MediatR;

namespace Fishie.Services.TelegramService.Commands;

/// <summary>
/// Get the message history from the channel by word. Example: /sendHistoryWords chat name | 5 | words
/// </summary>
internal class SendHistoryWordsCommand : Command, IRequest
{
    public static readonly string CommandText = "sendHistoryWords";
}