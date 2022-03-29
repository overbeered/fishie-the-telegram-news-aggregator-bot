using Fishie.Core.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;
using TL;
using WTelegram;

namespace Fishie.Services.TelegramService
{
    /// <summary>
    /// Message Handler
    /// </summary>
    public class MessagesHandler
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly СommandsHandler _сommandsHandler;

        public MessagesHandler(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _сommandsHandler = new СommandsHandler(_serviceScopeFactory);
        }

        /// <summary>
        /// Processes messages
        /// </summary>
        /// <param name="client">Сlient</param>
        /// <param name="channelId">Сhannel\chat id</param>
        /// <param name="idMessage">Message id</param>
        /// <param name="message">Message</param>
        /// <returns></returns>
        public async Task HandleAsync(Client client, long channelId, int idMessage, string message)
        {
            if (message.IndexOf("/") == 0) await _сommandsHandler.HandleAsync(client, channelId, message.Remove(0, 1));
            await ForwardMessagesAsync(client, channelId, idMessage);
        }

        /// <summary>
        /// Forward messages to chat
        /// </summary>
        /// <param name="client">Сlient</param>
        /// <param name="channelId">Сhannel\chat id</param>
        /// <param name="idMessage">Message id</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private async Task ForwardMessagesAsync(Client client, long channelId, int idMessage)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                IForwardMessagesRepository forwardMessagesRepository = scope.ServiceProvider
                    .GetRequiredService<IForwardMessagesRepository>();
                var listSendMessages = await forwardMessagesRepository.GetAllForwardMessagesAsync();

                if (listSendMessages != null)
                {
                    listSendMessages = listSendMessages!.Where(c => c!.ChannelId == channelId);
                    IChatRepository chatRepository = scope.ServiceProvider.GetRequiredService<IChatRepository>();
                    IChannelRepository channelRepository = scope.ServiceProvider.GetRequiredService<IChannelRepository>();

                    foreach (var sendMessages in listSendMessages)
                    {
                        var chat = await chatRepository.GetChatByIdAsync(sendMessages!.ChatId);

                        if (chat == null) throw new Exception();

                        var core = await channelRepository.GetChannelByIdAsync(channelId);

                        await client.Messages_ForwardMessages(new InputChannel()
                        { channel_id = core!.Id, access_hash = core.AccessHash },
                        new int[] { idMessage },
                        new long[] { Random.Shared.Next(int.MinValue, int.MaxValue) },
                        new InputChannel()
                        { channel_id = chat.Id, access_hash = chat.AccessHash });
                    }
                }
            }
        }
    }
}