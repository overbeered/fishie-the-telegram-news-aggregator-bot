using Fishie.Core.Repositories;
using Fishie.Services.TelegramService.Commands.ResponseCommands;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using TL;
using WTelegram;
using CoreModels = Fishie.Core.Models;

namespace Fishie.Services.TelegramService.Commands
{
    /// <summary>
    /// Find and add a channel\chat to the database. Example: /addChat chat name
    /// </summary>
    public class AddToChat : ICommand
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public AddToChat(IServiceScopeFactory serviceScopeFactory)
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
                        "Find and add a channel\\chat to the database. Example: /addChat chat name");
                }
            }
            else
            {
                var search = await client.Contacts_Search(action);

                if (search.chats.Count == 0)
                {
                    await new ResponseCommand(_serviceScopeFactory).ExecuteAsync(client,
                        chatId,
                        $"chat {action} not found");
                }
                else
                {
                    foreach (var (id, chat) in search.chats)
                    {
                        if (((Channel)chat).username == action || chat.Title == action)
                        {
                            var channel = (InputPeerChannel)chat.ToInputPeer();
                            var coreChat = new CoreModels.Chat(
                                    channel.channel_id,
                                    ((Channel)chat).username != null ? ((Channel)chat).username : chat.Title,
                                    channel.access_hash);
                            using (var scope = _serviceScopeFactory.CreateScope())
                            {
                                IChatRepository chatRepository = scope.ServiceProvider.GetRequiredService<IChatRepository>();
                                await chatRepository!.AddChatAsync(coreChat);
                            }
                            break;
                        }
                    }
                }
            }
        }
    }
}
