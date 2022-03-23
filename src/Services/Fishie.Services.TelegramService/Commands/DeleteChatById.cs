using Fishie.Core.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using WTelegram;

namespace Fishie.Services.TelegramService.Commands
{
    /// <summary>
    /// Delete from the database channel\chat. Example: /deleteChannelById 123456789
    /// </summary>
    internal class DeleteChatById : ICommand
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public DeleteChatById(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task ExecuteAsync(Client client, string action)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                IChatRepository chatRepository = scope.ServiceProvider.GetRequiredService<IChatRepository>();
                await chatRepository.DeleteChatByIdAsync(long.Parse(action));
            }
        }
    }
}
