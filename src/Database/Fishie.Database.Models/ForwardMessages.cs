﻿namespace Fishie.Database.Models
{
    /// <summary>
    /// Model for forwarding messages to a chat
    /// </summary>
    public class ForwardMessages
    {
        public int Id { get; set; }

        /// <summary>
        /// Chat model id
        /// </summary>
        public long ChatId { get; set; }

        /// <summary>
        /// Channel model id
        /// </summary>
        public long ChannelId { get; set; }
    }
}