using Fishie.Core.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Fishie.Services.Background.TelegramLoginBackgroundServices;

/// <summary>
/// Telegram connection/disconnection background service
/// </summary>
public class TelegramLoginBackgroundServices : IHostedService
{
    private readonly ILogger<TelegramLoginBackgroundServices> _logger;
    private readonly ITelegramServices _telegramServices;

    public TelegramLoginBackgroundServices(ILogger<TelegramLoginBackgroundServices> logger,
        ITelegramServices telegramServices)
    {
        _logger = logger;
        _telegramServices = telegramServices;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await _telegramServices.LoginAsync();

        if (!_telegramServices.Disconnected)
        {
            _logger.LogInformation("Services: {ServicesName}. Client login", nameof(TelegramLoginBackgroundServices));
        }
        else
        {
            _logger.LogError("Services: {ServicesName}. Client disconnected", nameof(TelegramLoginBackgroundServices));
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _telegramServices.Reset();
        _logger.LogInformation("Services: {ServicesName}. Client Reset", nameof(TelegramLoginBackgroundServices));
        return Task.CompletedTask;
    }
}