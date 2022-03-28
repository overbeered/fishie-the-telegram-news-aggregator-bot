﻿using Fishie.Core.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using TL;
using WTelegram;


namespace Fishie.Services.TelegramService.Commands
{
    /// <summary>
    /// The list of commands is sent to the chat. Example: /commands
    /// </summary>
    internal class ToCommands : ICommand
    {

        private readonly IServiceScopeFactory _serviceScopeFactory;

        public ToCommands(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task ExecuteAsync(Client client, long chatId, string action)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                IChatRepository chatRepository = scope.ServiceProvider.GetRequiredService<IChatRepository>();
                var chat = await chatRepository.GetChatByIdAsync(chatId);

                if (chat == null) throw new Exception();

                await client.SendMessageAsync(new InputChannel()
                {
                    channel_id = chat.Id,
                    access_hash = chat.AccessHash
                }, action);
            }
        }
    }
}
