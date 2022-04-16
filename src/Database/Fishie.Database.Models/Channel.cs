namespace Fishie.Database.Models
{
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
        public string? Username { get; private set; }

        /// <summary>
        ///  Telegram access hash channel
        /// </summary>
        public long AccessHash { get; set; }

#nullable disable
        public Channel() { }
#nullable restore 

        public Channel(long id, long accessHash, string? name, string? username)
        {
            Id = id;
            Name = name;
            Username = username;
            AccessHash = accessHash;
        }
    }
}
