using Fishie.Core.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fishie.Server.Configuration
{
    public static class AdminExtension
    {
        public static void AddAdmin(this IServiceCollection services, IConfiguration configuration)
        {
            var connectors = configuration.GetSection("AdminConfiguration").Get<AdminConfiguration>();

            services.AddTransient(_ => connectors);
        }
    }
}
