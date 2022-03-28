﻿using Fishie.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fishie.Database.Context.Configuration
{
    internal class ForwardMessagesConfiguration : IEntityTypeConfiguration<ForwardMessages>
    {
        public void Configure(EntityTypeBuilder<ForwardMessages> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.ChannelId);
            builder.Property(c => c.ChatId);
        }
    }
}