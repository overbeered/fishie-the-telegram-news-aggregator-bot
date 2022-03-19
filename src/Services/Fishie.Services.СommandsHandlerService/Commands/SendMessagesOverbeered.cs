using Fishie.Core.Services;

namespace Fishie.Services.СommandsHandlerService.Commands
{
    /// <summary>
    /// channel | 5
    /// </summary>
    internal class SendMessagesOverbeered : ICommand
    {
        private readonly IChannelOrChatServices _channelOrChatServices;

        public SendMessagesOverbeered(IChannelOrChatServices channelOrChatServices)
        {
            _channelOrChatServices = channelOrChatServices;
        }

        public async Task ExecuteAsync(string action)
        {
            var channel = action.Remove(action.IndexOf("|") - 1);
            var count = action.Remove(0, action.IndexOf("|") + 2);
            List<string?>? messagesList =
                await _channelOrChatServices.GetMessagesChannelAsync(channel, int.Parse(count));

            if (messagesList != null)
            {
                foreach (var message in messagesList)
                {
                    await _channelOrChatServices.SendMessagesChannelAsync("Overbeered", message! + " Name: " + channel);
                }
            }
        }
    }
}
