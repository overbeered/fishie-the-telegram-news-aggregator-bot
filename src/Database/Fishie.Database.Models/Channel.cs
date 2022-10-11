namespace Fishie.Database.Models;

/// <summary>
/// Сhannel model for the database
/// </summary>
public class Channel
{
    /// <summary>
    /// Telegram channel id
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Telegram channel name
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
    public Channel() { }

    public Channel(long id, long accessHash, string name, string username)
    {
        Id = id;
        AccessHash = accessHash;
        Name = name;
        Username = username;
    }
#nullable restore
}