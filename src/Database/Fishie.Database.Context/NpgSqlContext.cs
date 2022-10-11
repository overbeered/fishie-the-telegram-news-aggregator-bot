using Fishie.Database.Context.Configuration;
using Fishie.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace Fishie.Database.Context;

#nullable disable
public class NpgSqlContext : DbContext
{
    public DbSet<Channel> Channels { get; set; }
    public DbSet<Chat> Chats { get; set; }
    public DbSet<ForwardMessages> ForwardMessages { get; set; }
    public DbSet<Admin> Admins { get; set; }

    public NpgSqlContext(DbContextOptions<NpgSqlContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ChannelConfiguration());
        modelBuilder.ApplyConfiguration(new ChatConfiguration());
        modelBuilder.ApplyConfiguration(new ForwardMessagesConfiguration());
        modelBuilder.ApplyConfiguration(new AdminConfiguration());
    }
}
#nullable restore