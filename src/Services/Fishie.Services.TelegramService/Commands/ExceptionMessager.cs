using Fishie.Core.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using TL;
using WTelegram;

namespace Fishie.Services.TelegramService.Commands
{
    /// <summary>
    /// If the command is not found
    /// </summary>
    internal class ExceptionMessager : ICommand
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public ExceptionMessager(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task ExecuteAsync(Client client, long channelId, string action)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                IChatRepository chatRepository = scope.ServiceProvider.GetRequiredService<IChatRepository>();
                var chat = await chatRepository.GetChatByIdAsync(channelId);

                if (chat == null) throw new Exception();

                await client.SendMessageAsync(new InputChannel()
                {
                    channel_id = chat.Id,
                    access_hash = chat.AccessHash
                }, "There is no such command: " + action);
            }
        }
    }
}
