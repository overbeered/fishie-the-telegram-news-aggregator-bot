using Fishie.Core.Models;
using Fishie.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fishie.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChannelController : ControllerBase
    {
        private readonly IChannelServices _channelServices;
        private readonly ILogger<ChannelController> _logger;

        public ChannelController(IChannelServices channelServices,
            ILogger<ChannelController> logger)
        {
            _channelServices = channelServices;
            _logger = logger;
        }

        [HttpPost("AddChannel")]
        public async Task PostAddChannelAsync([FromBody] string value)
        {
            try
            {
                await _channelServices.AddChannelAsync(value);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error in Controller: {ControllerName} in Method: {MethodName},",
                    nameof(ChannelController),
                    nameof(PostAddChannelAsync));
            }
        }

        [HttpPost("DeleteChannel")]
        public async Task PostDeleteChannelAsync([FromBody] string value)
        {
            try
            {
                await _channelServices.DeleteChannelAsync(value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Controller: {ControllerName} in Method: {MethodName},",
                    nameof(ChannelController),
                    nameof(PostDeleteChannelAsync));
            }
        }

        [HttpPost("DeleteChannelById")]
        public async Task PostDeleteChannelByIdAsync([FromBody] long value)
        {
            try
            {
                await _channelServices.DeleteChannelByIdAsync(value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Controller: {ControllerName} in Method: {MethodName},",
                    nameof(ChannelController),
                    nameof(PostDeleteChannelByIdAsync));
            }
        }

        [HttpGet("AllChannels")]
        public async Task<IEnumerable<Channel?>?> GetAllChannelsAsync()
        {
            try
            {
                return await _channelServices.GetAllChannelsAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Controller: {ControllerName} in Method: {MethodName},",
                    nameof(ChannelController),
                    nameof(GetAllChannelsAsync));
            }

            return default;
        }

        [HttpPost("Channel")]
        public async Task<Channel?> PostChannelAsync([FromBody] string value)
        {
            try
            {
                return await _channelServices.GetChannelAsync(value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Controller: {ControllerName} in Method: {MethodName},",
                    nameof(ChannelController),
                    nameof(PostChannelAsync));
            }

            return default;
        }

        [HttpPost("ChannelById")]
        public async Task<Channel?> PostChannelByIdAsync([FromBody] long value)
        {
            try
            {
                return await _channelServices.GetChannelByIdAsync(value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Controller: {ControllerName} in Method: {MethodName},",
                    nameof(ChannelController),
                    nameof(PostChannelByIdAsync));
            }

            return default;
        }

        [HttpPost("Subscribe")]
        public async Task PostSubscribeAsync([FromBody] string value)
        {
            try
            {
                await _channelServices.SubscribeAsync(value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Controller: {ControllerName} in Method: {MethodName},",
                    nameof(ChannelController),
                    nameof(PostSubscribeAsync));
            }
        }

        [HttpPost("Unsubscribe")]
        public async Task PostUnsubscribeAsync([FromBody] string value)
        {
            try
            {
                await _channelServices.UnsubscribeAsync(value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Controller: {ControllerName} in Method: {MethodName},",
                    nameof(ChannelController),
                    nameof(PostUnsubscribeAsync));
            }
        }

        [HttpPost("Messages")]
        public async Task<List<string?>?> PostMessagesChannelAsync(string value, int count)
        {
            try
            {
                return await _channelServices.GetMessagesChannelAsync(value, count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Controller: {ControllerName} in Method: {MethodName},",
                    nameof(ChannelController),
                    nameof(PostMessagesChannelAsync));
            }

            return default;
        }

        [HttpPost("SendMessages")]
        public async Task PostSendMessagesChannelAsync(string value, string messages)
        {
            try
            {

                await _channelServices.SendMessagesChannelAsync(value, messages);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Controller: {ControllerName} in Method: {MethodName},",
                    nameof(ChannelController),
                    nameof(PostMessagesChannelAsync));
            }
        }

        [HttpPost("SendMessagesOverbeered")]
        public async Task PostSendMessagesChannelOverbeeredAsync(string value, int count)
        {
            try
            {
                List<string?>? messagesList = await _channelServices.GetMessagesChannelAsync(value, count);

                if(messagesList != null)
                {
                    foreach (var message in messagesList)
                    {
                        await _channelServices.SendMessagesChannelAsync("Overbeered", message! + " Name: " +value);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Controller: {ControllerName} in Method: {MethodName},",
                    nameof(ChannelController),
                    nameof(PostMessagesChannelAsync));
            }
        }
    }
}
