namespace Fishie.Core.Models;

/// <summary>
/// Model for forwarding messages to a chat
/// </summary>
public class ForwardMessages
{
    /// <summary>
    /// Channel model id
    /// </summary>
    public long ChannelId { get; private set; }

    /// <summary>
    /// Chat model id
    /// </summary>
    public long ChatId { get; private set; }

    public ForwardMessages(long channelId, long chatId)
    {
        ChannelId = channelId;
        ChatId = chatId;
    }
}