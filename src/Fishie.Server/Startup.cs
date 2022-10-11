using Fishie.Core.Repositories;
using Fishie.Core.Services;
using Fishie.Database.Repositories;
using Fishie.Server.Configuration;
using Fishie.Services.Background.TelegramLoginBackgroundServices;
using Fishie.Services.TelegramService;
using MediatR;
using Microsoft.OpenApi.Models;
using WTelegram;

namespace Fishie.Server;

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

        services.AddMediatR(typeof(TelegramServices).Assembly);

        services.AddSingleton<TelegramLoginBackgroundServices>();
        services.AddHostedService<TelegramLoginBackgroundServices>();

        services.AddSingleton<ITelegramServices, TelegramServices>();
        services.AddSingleton(r => { return new Client(ConfigTelegram.Config); });
        services.AddTransient<IDisposableResource, DisposableResource>();

        services.AddTransient<IChatRepository, ChatRepository>();
        services.AddTransient<IChannelRepository, ChannelRepository>();
        services.AddTransient<IForwardMessagesRepository, ForwardMessagesRepository>();
        services.AddTransient<IAdminRepository, AdminRepository>();

        services.AddDbConfiugration(Configuration);
        services.AddAdminConfiugration(Configuration);
        services.AddChatConfiugration(Configuration);
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