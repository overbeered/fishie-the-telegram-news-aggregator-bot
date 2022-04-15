using Fishie.Core.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using TL;
using WTelegram;

namespace Fishie.Services.TelegramService.Commands.Utils
{
    /// <summary>
    /// Chat Response command
    /// </summary>
    internal static class CommandResponseHelper
    {
        public static async Task ExecuteAsync(IServiceScopeFactory serviceScopeFactory, Client client, long chatId, string action)
        {
            using (var scope = serviceScopeFactory.CreateScope())
            {
                IChatRepository chatRepository = scope.ServiceProvider.GetRequiredService<IChatRepository>();
                var chat = await chatRepository.FindChatByIdAsync(chatId);

                if (chat == null) throw new Exception("chat not found");

                await client.SendMessageAsync(new InputChannel()
                {
                    channel_id = chat.Id,
                    access_hash = chat.AccessHash
                }, action);
            }
        }
    }
}
