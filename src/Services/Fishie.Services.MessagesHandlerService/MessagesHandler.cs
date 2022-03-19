using Fishie.Core.Services;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Fishie.Services.MessagesHandlerService
{

    public class MessagesHandler : IMessagesHandler
    {
        private readonly ILogger<MessagesHandler> _logger;
        private readonly IСommandsHandler _сommandsHandler;

        public MessagesHandler(ILogger<MessagesHandler> logger,
            IСommandsHandler сommandsHandler)
        {
            _logger = logger;
            _сommandsHandler = сommandsHandler;
        }

        public async Task Handle(string message)
        {
            // /command
            if (message.IndexOf("/") == 0) await _сommandsHandler.HandleAsync(message.Remove(0, 1));
        }
    }
}