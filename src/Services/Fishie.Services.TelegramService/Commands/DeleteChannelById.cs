using Fishie.Core.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using WTelegram;

namespace Fishie.Services.TelegramService.Commands
{
    /// <summary>
    /// Delete from the database channel\chat. Example: /deleteChannelById 123456789
    /// </summary>
    internal class DeleteChannelById : ICommand
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public DeleteChannelById(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task ExecuteAsync(Client client, string action)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                IChannelRepository chatRepository = scope.ServiceProvider.GetRequiredService<IChannelRepository>();
                await chatRepository.DeleteChannelByIdAsync(long.Parse(action));
            }
        }
    }
}
