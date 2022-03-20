using Fishie.Core.Services;

namespace Fishie.Services.СommandsHandlerService.Commands
{
    internal class Unsubscribe : ICommand
    {
        private readonly IChatServices _channelOrChatServices;

        public Unsubscribe(IChatServices channelOrChatServices)
        {
            _channelOrChatServices = channelOrChatServices;
        }

        public async Task ExecuteAsync(string action)
        {
            await _channelOrChatServices.UnsubscribeAsync(action);
        }
    }
}
