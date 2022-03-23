using Fishie.Core.Services;
using Fishie.Services.TelegramService;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace TelegramLoginBackgroundService
{
    public class TelegramLoginBackgroundServices : IHostedService
    {
        private readonly ILogger<TelegramServices> _logger;
        private readonly ITelegramServices _telegramServices;

        public TelegramLoginBackgroundServices(ILogger<TelegramServices> logger,
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
}
