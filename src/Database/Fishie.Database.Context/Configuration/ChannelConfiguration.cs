using Fishie.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fishie.Database.Context.Configuration
{
    internal class ChannelConfiguration : IEntityTypeConfiguration<Channel>
    {
        public void Configure(EntityTypeBuilder<Channel> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id);
            builder.Property(c => c.Name);
            builder.Property(c => c.AccessHash);
        }
    }
}
