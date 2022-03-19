using Fishie.Core.Services;

namespace Fishie.Services.СommandsHandlerService.Commands
{
    internal class Subscribe : ICommand
    {
        private readonly IChannelOrChatServices _channelOrChatServices;

        public Subscribe(IChannelOrChatServices channelOrChatServices)
        {
            _channelOrChatServices = channelOrChatServices;
        }

        public async Task ExecuteAsync(string action)
        {
            await _channelOrChatServices.SubscribeAsync(action);
        }
    }
}
