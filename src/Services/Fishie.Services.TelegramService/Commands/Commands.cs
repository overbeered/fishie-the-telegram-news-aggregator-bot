using Fishie.Core.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using WTelegram;
using TL;


namespace Fishie.Services.TelegramService.Commands
{
    internal class ToCommands : ICommand
    {

        private readonly IServiceScopeFactory _serviceScopeFactory;

        public ToCommands(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task ExecuteAsync(Client client, string action)
        {
            var chatName = action.Remove(action.IndexOf("\n"));
            var message = action.Remove(0, action.IndexOf("\n") + 9);

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
