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
        ///  Telegram access hash channel
        /// </summary>
        public long AccessHash { get; set; }

#nullable disable
        public Chat() { }
#nullable restore 

        public Chat(long id,
            string? name,
            long accessHash)
        {
            Id = id;
            Name = name;
            AccessHash = accessHash;
        }
    }
}
