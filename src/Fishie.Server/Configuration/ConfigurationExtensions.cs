using Fishie.Core.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fishie.Server.Configuration
{
    internal static class ConfigurationExtensions
    {
        public static void AddConnectorsConfiugration(this IServiceCollection services, IConfiguration configuration)
        {
            var connectors = configuration.GetSection("ClientConfiguration").Get<СlientConfiguration>();

            services.AddTransient(_ => connectors);
        }
        public static void AddAdminConfiugration(this IServiceCollection services, IConfiguration configuration)
        {
            var connectors = configuration.GetSection("AdminConfiguration").Get<AdminConfiguration>();

            services.AddTransient(_ => connectors);
        }

        public static void AddChatConfiugration(this IServiceCollection services, IConfiguration configuration)
        {
            var connectors = configuration.GetSection("ChatConfiguration").Get<ChatConfiguration>();

            services.AddTransient(_ => connectors);
        }
    }
}
