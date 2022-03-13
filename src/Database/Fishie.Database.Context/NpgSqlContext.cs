using Fishie.Database.Context.Configuration;
using Fishie.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace Fishie.Database.Context
{
    public class NpgSqlContext : DbContext
    {
        public DbSet<Channel>? Channels { get; set; }

        public NpgSqlContext(DbContextOptions<NpgSqlContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ChannelConfiguration());
        }
    }
}
