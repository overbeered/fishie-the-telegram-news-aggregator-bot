using Fishie.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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


        //[HttpGet]
        //public async Task<IEnumerable<string>> Get()
        //{
            

        //    return new string[] { "value1", "value2" };
        //}

        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        [HttpPost]
        public async Task Post([FromBody] string value)
        {
            await _channelServices.AddChannelAsync(value);
        }

        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
