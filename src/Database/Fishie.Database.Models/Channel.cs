namespace Fishie.Database.Models
{
    /// <summary>
    /// Сhannel model for the database
    /// </summary>
    public class Channel
    {
        /// <summary>
        /// Telegram id channel
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Telegram name channel
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        ///  Telegram access hash channel
        /// </summary>
        public long AccessHash { get; set; }

#nullable disable
        public Channel() { }
#nullable restore 

        public Channel(long id,
            string? name,
            long accessHash)
        {
            Id = id;
            Name = name;
            AccessHash = accessHash;
        }
    }
}
