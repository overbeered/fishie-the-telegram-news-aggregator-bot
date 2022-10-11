namespace Fishie.Database.Models;

/// <summary>
/// Chat model for the database
/// </summary>
public class Chat
{
    /// <summary>
    /// Telegram chat id
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Telegram chat name
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Telegram channel username
    /// </summary>
    public string? Username { get; set; }

    /// <summary>
    ///  Telegram access hash channel
    /// </summary>
    public long AccessHash { get; set; }

    public ICollection<ForwardMessages> ForwardMessages { get; set; }

#nullable disable
    public Chat() { }

    public Chat(long id, long accessHash, string name, string username)
    {
        Id = id;
        AccessHash = accessHash;
        Name = name;
        Username = username;
    }
#nullable restore
}