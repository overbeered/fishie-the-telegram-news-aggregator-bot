﻿namespace Fishie.Database.Models
{
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
    }
}