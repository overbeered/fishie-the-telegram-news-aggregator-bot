using Fishie.Core.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using WTelegram;

namespace Fishie.Services.TelegramService.Commands
{
    /// <summary>
    /// Delete from the database chat. Example: /deleteChat chat name
    /// </summary>
    internal class DeleteChat : ICommand
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public DeleteChat(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task ExecuteAsync(Client client, string action)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                IChatRepository chatRepository = scope.ServiceProvider.GetRequiredService<IChatRepository>();
                await chatRepository.DeleteChatAsync(action);
            }
        }
    }
}
