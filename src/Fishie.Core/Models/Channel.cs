namespace Fishie.Core.Models
{
    /// <summary>
    /// Telegram channel
    /// </summary>
    public class Channel
    {
        /// <summary>
        /// Telegram id channel
        /// </summary>
        public long Id { get; private set; }

        /// <summary>
        /// Telegram name channel
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        ///  Telegram access hash channel
        /// </summary>
        public long AccessHash { get; private set; }


        public Channel(long id, string name, long accessHash)
        {
            Id = id;
            Name = name;
            AccessHash = accessHash;
        }
    }
}
