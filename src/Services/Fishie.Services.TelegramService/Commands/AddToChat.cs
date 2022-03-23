using Fishie.Core.Repositories;
using System.Threading.Tasks;
using WTelegram;
using TL;
using CoreModels = Fishie.Core.Models;
using System;
using Microsoft.Extensions.DependencyInjection;

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

        public async Task ExecuteAsync(Client client, string action)
        {
            var search = await client.Contacts_Search(action);
            if (search == null) throw new Exception($"channel {search} not found");
            
            foreach (var (id, chat) in search.chats)
            {
                if (chat.Title == action)
                {
                    var channel = (InputPeerChannel)chat.ToInputPeer();
                    var coreChat = new CoreModels.Chat(
                            channel.channel_id,
                            chat.Title,
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
