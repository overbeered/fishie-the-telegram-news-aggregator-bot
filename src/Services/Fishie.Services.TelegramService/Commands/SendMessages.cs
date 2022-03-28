using Fishie.Core.Repositories;
using Fishie.Services.TelegramService.Commands.ResponseCommands;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using TL;
using WTelegram;

namespace Fishie.Services.TelegramService.Commands
{
    /// <summary>
    /// Send a message to the chat from the database. Example: /sendMessages channel name message: Hello world!
    /// </summary>
    internal class SendMessages : ICommand
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public SendMessages(IServiceScopeFactory serviceScopeFactory)
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
                        "Send a message to the chat from the database. Example: /sendMessages channel name message: Hello world!");
                }
            }
            else
            {
                var chatName = action.Remove(action.IndexOf("message: ") - 1);
                var message = action.Remove(0, action.IndexOf("message: ") + 9);

                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    IChatRepository chatRepository = scope.ServiceProvider.GetRequiredService<IChatRepository>();
                    var chat = await chatRepository.GetChatAsync(chatName);

                    if (chat == null)
                    {
                        await new ResponseCommand(_serviceScopeFactory).ExecuteAsync(client,
                            chatId,
                            $"chat {action} not found in the database");
                    }
                    else
                    {
                        await client.SendMessageAsync(new InputChannel()
                        {
                            channel_id = chat.Id,
                            access_hash = chat.AccessHash
                        }, message);
                    }
                }
            }
        }
    }
}
