using Fishie.Core.Repositories;
using Fishie.Services.TelegramService.Commands.ResponseCommands;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using WTelegram;

namespace Fishie.Services.TelegramService.Commands
{
    /// <summary>
    /// Delete from the database chat. Example: /deleteChat chat name
    /// </summary>
    internal class DeleteToChat : ICommand
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public DeleteToChat(IServiceScopeFactory serviceScopeFactory)
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
                        "Delete from the database chat. Example: /deleteChat chat name");
                }
            }
            else
            {
                //Think about how to delete from ForwardMessages
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    IChatRepository chatRepository = scope.ServiceProvider.GetRequiredService<IChatRepository>();
                    await chatRepository.DeleteChatAsync(action);
                }
            }
        }
    }
}
