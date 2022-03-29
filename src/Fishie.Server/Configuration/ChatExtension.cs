﻿using Fishie.Core.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fishie.Server.Configuration
{
    internal static class ChatExtension
    {
        public static void AddChat(this IServiceCollection services, IConfiguration configuration)
        {
            var connectors = configuration.GetSection("ChatConfiguration").Get<ChatConfiguration>();

            services.AddTransient(_ => connectors);
        }
    }
}
