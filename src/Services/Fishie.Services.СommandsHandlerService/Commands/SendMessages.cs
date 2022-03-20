using Fishie.Core.Services;

namespace Fishie.Services.СommandsHandlerService.Commands
{
    /// <summary>
    /// channel message:
    /// </summary>
    internal class SendMessages : ICommand
    {
        private readonly IChatServices _channelOrChatServices;

        public SendMessages(IChatServices channelOrChatServices)
        {
            _channelOrChatServices = channelOrChatServices;
        }

        public async Task ExecuteAsync(string action)
        {
            var channel = action.Remove(action.IndexOf("message: ") - 1);
            var message = action.Remove(0, action.IndexOf("message: ") + 9);
            
            await _channelOrChatServices.SendMessagesChannelAsync(channel, message);
        }
    }
}
