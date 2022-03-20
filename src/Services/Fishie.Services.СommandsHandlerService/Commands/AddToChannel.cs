using Fishie.Core.Services;

namespace Fishie.Services.СommandsHandlerService.Commands
{
    internal class AddToChannel : ICommand
    {
        private readonly IChatServices _channelOrChatServices;

        public AddToChannel(IChatServices channelOrChatServices)
        {
            _channelOrChatServices = channelOrChatServices;
        }

        public async Task ExecuteAsync(string action)
        {
            await _channelOrChatServices.AddChannelAsync(action);
        }
    }
}
