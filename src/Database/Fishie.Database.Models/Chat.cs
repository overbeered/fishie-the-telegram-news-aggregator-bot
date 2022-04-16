namespace Fishie.Database.Models
{
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
        public string? Username { get; private set; }

        /// <summary>
        ///  Telegram access hash channel
        /// </summary>
        public long AccessHash { get; set; }

#nullable disable
        public Chat() { }
#nullable restore 

        public Chat(long id, long accessHash, string? name, string? username)
        {
            Id = id;
            Name = name;
            Username = username;
            AccessHash = accessHash;
        }
    }
}
