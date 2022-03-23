using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using WTelegram;

namespace Fishie.Services.TelegramService
{
    public class MessagesHandler 
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public MessagesHandler(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task Handle(Client client, string message)
        {
            // /command
            if (message.IndexOf("/") == 0) await new СommandsHandler(_serviceScopeFactory).HandleAsync(client, message.Remove(0, 1));
        }
    }
}