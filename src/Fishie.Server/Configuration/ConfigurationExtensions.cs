using Fishie.Core.Configuration;
using Fishie.Database.Context;
using Microsoft.EntityFrameworkCore;

namespace Fishie.Server.Configuration;

internal static class ConfigurationExtensions
{
    public static void AddDbConfiugration(this IServiceCollection services, IConfiguration configuration)
    {
        var connection = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING") ??
                       configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<NpgSqlContext>(options => options.UseNpgsql(connection));

        services.AddTransient(_ => connection);
    }
    public static void AddAdminConfiugration(this IServiceCollection services, IConfiguration configuration)
    {
        var admin = configuration.GetSection("AdminConfiguration").Get<AdminConfiguration>();

        services.AddTransient(_ => admin);
    }

    public static void AddChatConfiugration(this IServiceCollection services, IConfiguration configuration)
    {
        var chat = configuration.GetSection("ChatConfiguration").Get<ChatConfiguration>();

        services.AddTransient(_ => chat);
    }
}