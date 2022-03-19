using Fishie.Core.Services;

namespace Fishie.Services.СommandsHandlerService.Commands
{
    internal class AddToChannel : ICommand
    {
        private readonly IChannelOrChatServices _channelOrChatServices;

        public AddToChannel(IChannelOrChatServices channelOrChatServices)
        {
            _channelOrChatServices = channelOrChatServices;
        }

        public async Task ExecuteAsync(string action)
        {
            await _channelOrChatServices.AddChannelAsync(action);
        }
    }
}
