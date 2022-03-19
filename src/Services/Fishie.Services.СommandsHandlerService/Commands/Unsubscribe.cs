using Fishie.Core.Services;

namespace Fishie.Services.СommandsHandlerService.Commands
{
    internal class Unsubscribe : ICommand
    {
        private readonly IChannelOrChatServices _channelOrChatServices;

        public Unsubscribe(IChannelOrChatServices channelOrChatServices)
        {
            _channelOrChatServices = channelOrChatServices;
        }

        public async Task ExecuteAsync(string action)
        {
            await _channelOrChatServices.UnsubscribeAsync(action);
        }
    }
}
