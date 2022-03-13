using Fishie.Core.Connectors;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fishie.Server.Providers
{
    internal static class ConnectorsClientProviderExtension
    {
        public static void AddConnectors(this IServiceCollection services, IConfiguration configuration)
        {
            var connectors = configuration.GetSection("ClientConfiguration").Get<СlientConnector>();
            
            services.AddTransient(_ => connectors);
        }
    }
}
