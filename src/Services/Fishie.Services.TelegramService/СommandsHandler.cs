using Fishie.Services.TelegramService.Commands;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WTelegram;

namespace Fishie.Services.TelegramService
{
    /// <summary>
    /// Command handler
    /// </summary>
    public class СommandsHandler
    {
        private readonly Dictionary<string, ICommand> _handlers;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public СommandsHandler(IServiceScopeFactory serviceScopeFactory)
        {
            _handlers = new Dictionary<string, ICommand>()
            {
                {"commands", new ToCommands(serviceScopeFactory) },
                { "addAdmin", new AddToAdmin(serviceScopeFactory)},
                {"forward", new Forward(serviceScopeFactory) },
                {"addChannel", new AddToChannel(serviceScopeFactory) },
                {"addChat", new AddToChat(serviceScopeFactory) },
                {"deleteChannel",  new DeleteToChannel(serviceScopeFactory)},
                {"deleteChat",  new DeleteToChat(serviceScopeFactory)},
                {"getAllChannels",  new GetAllChannels(serviceScopeFactory)},
                {"subscribe",  new Subscribe(serviceScopeFactory)},
                {"unsubscribe",  new Unsubscribe(serviceScopeFactory)},
                {"sendMessages",  new SendMessages(serviceScopeFactory)},
                {"sendMessagesHistory",  new SendMessagesHistory(serviceScopeFactory)},
            };
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task HandleAsync(Client client, long channelId, string command)
        {
            try
            {
                if ("commands" == command)
                {
                    string commandKey = " ";
                    foreach (var key in _handlers.Keys)
                    {
                        commandKey += "\n" + key;
                    }

                    await _handlers[command].ExecuteAsync(client, channelId, commandKey);
                }
                else
                {
                    if (command.IndexOf(" ") != -1)
                    {
                        var commandRemove = command.Remove(command.IndexOf(" "));
                        var action = command.Remove(0, command.IndexOf(" ") + 1);
                        await _handlers[commandRemove].ExecuteAsync(client, channelId, action);
                    }
                    else
                    {
                        await _handlers[command].ExecuteAsync(client, channelId, "");
                    }
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                var error = new ExceptionMessager(_serviceScopeFactory);
                await error.ExecuteAsync(client, channelId, command);
                throw new ArgumentOutOfRangeException();
            }
        }
    }
}
