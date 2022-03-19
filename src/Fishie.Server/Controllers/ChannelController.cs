using Fishie.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Fishie.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChannelController : ControllerBase
    {

        private readonly ILogger<ChannelController> _logger;
        private readonly IMessagesHandler _messagesHandler;

        public ChannelController(ILogger<ChannelController> logger,
            IMessagesHandler messagesHandler)
        {

            _logger = logger;
            _messagesHandler = messagesHandler;
        }

        [HttpPost("AddChannel")]
        public async Task PostAddChannelAsync([FromBody] string value)
        {
            try
            {
                await _messagesHandler.Handle(value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Controller: {ControllerName} in Method: {MethodName},",
                    nameof(ChannelController),
                    nameof(PostAddChannelAsync));
            }
        }
    }
}
