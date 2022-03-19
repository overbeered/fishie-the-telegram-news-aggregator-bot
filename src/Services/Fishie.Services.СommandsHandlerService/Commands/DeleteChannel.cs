using Fishie.Core.Services;

namespace Fishie.Services.СommandsHandlerService.Commands
{
    internal class DeleteChannel : ICommand
    {
        private readonly IChannelOrChatServices _channelOrChatServices;

        public DeleteChannel(IChannelOrChatServices channelOrChatServices)
        {
            _channelOrChatServices = channelOrChatServices;
        }

        public async Task ExecuteAsync(string action)
        {
            await _channelOrChatServices.DeleteChannelAsync(action);
        }
    }
}
