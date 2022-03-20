using Fishie.Core.Services;

namespace Fishie.Services.СommandsHandlerService.Commands
{
    internal class DeleteChannel : ICommand
    {
        private readonly IChatServices _channelOrChatServices;

        public DeleteChannel(IChatServices channelOrChatServices)
        {
            _channelOrChatServices = channelOrChatServices;
        }

        public async Task ExecuteAsync(string action)
        {
            await _channelOrChatServices.DeleteChannelAsync(action);
        }
    }
}
