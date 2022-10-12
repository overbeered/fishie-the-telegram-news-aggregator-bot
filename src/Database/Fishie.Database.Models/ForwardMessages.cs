namespace Fishie.Database.Models;

/// <summary>
/// Model for forwarding messages to a chat
/// </summary>
public class ForwardMessages
{
    public int Id { get; set; }

    /// <summary>
    /// Chat model id
    /// </summary>
    public long ChatId { get; set; }

    /// <summary>
    /// Channel model id
    /// </summary>
    public long ChannelId { get; set; }

    /// <summary>
    /// Chat model relationship "one to many"
    /// </summary>
    public Chat Chat { get; set; }

    /// <summary>
    /// Channel model relationship "one to many"
    /// </summary>
    public Channel Channel { get; set; }

#nullable disable
    public ForwardMessages() { }

    public ForwardMessages(long channelId, long chatId)
    {
        ChannelId = channelId;
        ChatId = chatId;
    }
#nullable restore
}