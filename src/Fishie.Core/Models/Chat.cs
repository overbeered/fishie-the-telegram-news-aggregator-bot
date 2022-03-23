namespace Fishie.Core.Models
{
    /// <summary>
    /// Telegram chat listening model
    /// </summary>
    public class Chat
    {
        /// <summary>
        /// Telegram chat Id
        /// </summary>
        public long Id { get; private set; }

        /// <summary>
        /// Telegram chat name
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        ///  Telegram access hash chat
        /// </summary>
        public long AccessHash { get; private set; }

        public Chat(long id, string name, long accessHash)
        {
            Id = id;
            Name = name;
            AccessHash = accessHash;
        }
    }
}
