using Fishie.Core.Configuration;
using Fishie.Database.Context;
using Microsoft.EntityFrameworkCore;

namespace Fishie.Server.Configuration;

internal static class ConfigurationExtensions
{
    /// <summary>
    /// Adds the DB context from the configuration
    /// </summary>
    public static void AddDbConfiugration(this IServiceCollection services, IConfiguration configuration)
    {
        var connection = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING") ??
                       configuration.GetConnectionString("DefaultConnection");
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        services.AddDbContext<NpgSqlContext>(options => options.UseNpgsql(connection));
    }

    /// <summary>
    /// Adds the AdminConfiguration dependency from the configuration
    /// </summary>
    public static void AddAdminConfiugration(this IServiceCollection services, IConfiguration configuration)
    {
        var admin = configuration.GetSection("AdminConfiguration").Get<AdminConfiguration>();

        services.AddTransient(_ => admin);
    }

    /// <summary>
    /// Adds the ChatConfiguration dependency from the configuration
    /// </summary>
    public static void AddChatConfiugration(this IServiceCollection services, IConfiguration configuration)
    {
        var chat = configuration.GetSection("ChatConfiguration").Get<ChatConfiguration>();

        services.AddTransient(_ => chat);
    }
}