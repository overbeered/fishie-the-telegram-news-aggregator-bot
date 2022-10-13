using Fishie.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fishie.Database.Context.Configuration;

/// <summary>
/// Configuration of the model for forwarding messages to a chat
/// </summary>
internal class ForwardMessagesConfiguration : IEntityTypeConfiguration<ForwardMessages>
{
    public void Configure(EntityTypeBuilder<ForwardMessages> builder)
    {
        builder.HasKey(c => c.Id);

        builder.HasOne(c => c.Chat).WithMany(f => f.ForwardMessages).HasForeignKey(cc => cc.ChatId);
        builder.HasOne(c => c.Channel).WithMany(f => f.ForwardMessages).HasForeignKey(cc => cc.ChannelId);
    }
}