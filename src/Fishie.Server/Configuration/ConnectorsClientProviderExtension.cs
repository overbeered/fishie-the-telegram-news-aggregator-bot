using Fishie.Core.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fishie.Server.Configuration
{
    internal static class ConnectorsClientProviderExtension
    {
        public static void AddConnectors(this IServiceCollection services, IConfiguration configuration)
        {
            var connectors = configuration.GetSection("ClientConfiguration").Get<СlientConfiguration>();

            services.AddTransient(_ => connectors);
        }
    }
}
