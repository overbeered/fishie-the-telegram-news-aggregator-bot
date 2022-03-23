using Fishie.Core.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
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

        public async Task ExecuteAsync(Client client, string action)
        {
            var chatName = action.Remove(action.IndexOf("message: ") - 1);
            var message = action.Remove(0, action.IndexOf("message: ") + 9);

            using (var scope = _serviceScopeFactory.CreateScope())
            {
                IChatRepository chatRepository = scope.ServiceProvider.GetRequiredService<IChatRepository>();
                var chat = await chatRepository.GetChatAsync(chatName);

                if (chat == null) throw new Exception();

                await client.SendMessageAsync(new InputChannel()
                {
                    channel_id = chat.Id,
                    access_hash = chat.AccessHash
                }, message);
            }

        }
    }
}
