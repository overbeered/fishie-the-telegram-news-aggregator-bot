using Fishie.Core.Configurat;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fishie.Server.Configurat
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
