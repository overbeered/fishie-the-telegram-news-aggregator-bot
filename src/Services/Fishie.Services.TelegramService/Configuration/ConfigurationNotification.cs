using MediatR;
using WTelegram;

namespace Fishie.Services.TelegramService.Configuration
{
    internal class ConfigurationNotification : INotification
    {
        public Client? Client { get; set; }
    }
}
