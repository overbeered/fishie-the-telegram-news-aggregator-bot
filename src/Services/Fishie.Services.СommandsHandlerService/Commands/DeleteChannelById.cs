using Fishie.Core.Services;

namespace Fishie.Services.СommandsHandlerService.Commands
{
    internal class DeleteChannelById : ICommand
    {
        private readonly IChannelOrChatServices _channelOrChatServices;

        public DeleteChannelById(IChannelOrChatServices channelOrChatServices)
        {
            _channelOrChatServices = channelOrChatServices;
        }

        public async Task ExecuteAsync(string action)
        {
            await _channelOrChatServices.DeleteChannelByIdAsync(long.Parse(action));
        }
    }
}
