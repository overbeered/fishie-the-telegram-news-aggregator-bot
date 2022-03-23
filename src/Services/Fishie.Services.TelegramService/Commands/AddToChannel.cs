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
    /// Find and add a channel\chat to the database. Example: /addChannel channel name
    /// </summary>
    public class AddToChannel : ICommand
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public AddToChannel(IServiceScopeFactory serviceScopeFactory)
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
                    var coreChannel = new CoreModels.Channel(
                            channel.channel_id,
                            chat.Title,
                            channel.access_hash);
                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        IChannelRepository chatRepository = scope.ServiceProvider.GetRequiredService<IChannelRepository>();
                        await chatRepository!.AddChannelAsync(coreChannel);
                    }
                    break;
                }
            }
        }
    }
}
