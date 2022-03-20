using Fishie.Core.Services;
using Fishie.Services.СommandsHandlerService.Commands;

namespace Fishie.Services.СommandsHandlerService
{
    public class СommandsHandler : IСommandsHandler
    {
        private readonly IChatServices _channelOrChatServices;
        private readonly Dictionary<string, ICommand> _handlers; 

        public СommandsHandler(IChatServices channelOrChatServices)
        {
            _channelOrChatServices = channelOrChatServices;

            _handlers = new Dictionary<string, ICommand>()
            {
                {"addChannel",  new AddToChannel(_channelOrChatServices)},  
                {"deleteChannel",  new DeleteChannel(_channelOrChatServices)},
                {"deleteChannelById",  new DeleteChannelById(_channelOrChatServices)},
                {"subscribe",  new Subscribe(_channelOrChatServices)},
                {"unsubscribe",  new Unsubscribe(_channelOrChatServices)},
                {"sendMessages",  new SendMessages(_channelOrChatServices)},
                {"sendMessagesOverbeered",  new SendMessagesOverbeered(_channelOrChatServices)}
            };
        }

        public async Task HandleAsync(string command)
        {
            var commandRemove = command.Remove(command.IndexOf(" "));
            var action = command.Remove(0, command.IndexOf(" ") + 1);
            
            await _handlers[commandRemove].ExecuteAsync(action);
        }
    }
}
