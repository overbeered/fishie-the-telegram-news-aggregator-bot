using Fishie.Services.TelegramService.Commands;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Threading.Tasks;
using WTelegram;

namespace Fishie.Services.TelegramService
{
    public class СommandsHandler
    {
        private readonly Dictionary<string, ICommand> _handlers;
        public СommandsHandler(IServiceScopeFactory serviceScopeFactory)
        {
            _handlers = new Dictionary<string, ICommand>()
            {
                {"commands", new ToCommands(serviceScopeFactory) },
                {"addChannel", new AddToChannel(serviceScopeFactory) },
                {"addChat", new AddToChat(serviceScopeFactory) },
                {"deleteChannel",  new DeleteChannel(serviceScopeFactory)},
                {"deleteChannelById",  new DeleteChannelById(serviceScopeFactory)},
                {"deleteChat",  new DeleteChat(serviceScopeFactory)},
                {"deleteChatById",  new DeleteChatById(serviceScopeFactory)},
                {"getAllChannels",  new GetAllChannels(serviceScopeFactory)},
                {"subscribe",  new Subscribe(serviceScopeFactory)},
                {"unsubscribe",  new Unsubscribe(serviceScopeFactory)},
                {"sendMessages",  new SendMessages(serviceScopeFactory)},
                {"sendMessagesOverbeered",  new SendMessagesOverbeered(serviceScopeFactory)}
            };
        }

        public async Task HandleAsync(Client client, string command)
        {
            var commandRemove = command.Remove(command.IndexOf(" "));
            var action = command.Remove(0, command.IndexOf(" ") + 1);
            
            if ("commands" == commandRemove)
            {
                string commandKey = action;
                foreach (var key in _handlers.Keys)
                {
                    commandKey += "\n" + key;
                }

                await _handlers[commandRemove].ExecuteAsync(client, commandKey);
            }
            else
            {
                await _handlers[commandRemove].ExecuteAsync(client, action);
            }
            
        }
    }
}
