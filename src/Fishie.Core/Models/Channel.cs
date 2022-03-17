namespace Fishie.Core.Models
{
    /// <summary>
    /// Telegram channel model
    /// </summary>
    public class Channel
    {
        /// <summary>
        /// Telegram channel Id
        /// </summary>
        public long Id { get; private set; }

        /// <summary>
        /// Telegram channel name
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
