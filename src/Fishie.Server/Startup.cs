using Fishie.Core.Repositories;
using Fishie.Core.Services;
using Fishie.Database.Context;
using Fishie.Database.Repositories;
using Fishie.Server.Providers;
using Fishie.Services.ChannelService;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using Microsoft.EntityFrameworkCore;
using Fishie.Services.TelegramService;
using Fishie.Services.MessagesHandlerService;
using Fishie.Services.СommandsHandlerService;

namespace Fishie.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Fishie.Server", Version = "v1" });
            });


            var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING") ??
                       Configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<NpgSqlContext>(options => options.UseNpgsql(connectionString));


            services.AddTransient<IChannelOrChatServices, ChannelOrChatServices>();
            services.AddTransient<IChannelRepository, ChannelRepository>();
            services.AddTransient<IMessagesHandler, MessagesHandler>();
            services.AddTransient<IСommandsHandler, СommandsHandler>();
            services.AddSingleton<ITelegramServices, TelegramServices>();
            services.AddConnectors(Configuration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Fishie.Server v1"));
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
