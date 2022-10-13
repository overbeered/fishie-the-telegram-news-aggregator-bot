using Fishie.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fishie.Database.Context.Configuration;

/// <summary>
/// Configuration of the administrator model for chat
/// </summary>
internal class AdminConfiguration : IEntityTypeConfiguration<Admin>
{
    public void Configure(EntityTypeBuilder<Admin> builder)
    {
        builder.HasKey(a => a.Id);

        builder.Property(a => a.Id);
        builder.Property(a => a.FirstName);
        builder.Property(a => a.LastName);
        builder.Property(a => a.Username);
    }
}