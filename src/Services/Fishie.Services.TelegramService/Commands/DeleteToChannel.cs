using Fishie.Core.Repositories;
using Fishie.Services.TelegramService.Commands.ResponseCommands;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Threading.Tasks;
using WTelegram;

namespace Fishie.Services.TelegramService.Commands
{
    /// <summary>
    /// Delete from the database channel\chat. Example: /deleteChannel channel name
    /// </summary>
    internal class DeleteToChannel : ICommand
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public DeleteToChannel(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task ExecuteAsync(Client client, long chatId, string action)
        {
            if (action.IndexOf("--info") != -1)
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    await new ResponseCommand(_serviceScopeFactory).ExecuteAsync(client,
                        chatId,
                        "Delete from the database channel\\chat. Example: /deleteChannel channel name");
                }
            }
            else
            {
                //Think about how to delete from ForwardMessages
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    IChannelRepository chatRepository = scope.ServiceProvider.GetRequiredService<IChannelRepository>();
                    await chatRepository.DeleteChannelAsync(action);
                }
            }
        }
    }
}
