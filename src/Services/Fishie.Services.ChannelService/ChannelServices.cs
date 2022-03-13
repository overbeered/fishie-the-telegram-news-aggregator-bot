using Fishie.Core.Repositories;
using Fishie.Core.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fishie.Core.Models;


namespace Fishie.Services.ChannelService
{
    public class ChannelServices : IChannelServices
    {
        private readonly ILogger<ChannelServices> _logger;
        private readonly IChannelRepository _channelRepository;
        private readonly ITelegramServices _telegramConnectorServices;
        public ChannelServices(ILogger<ChannelServices> logger,
            IChannelRepository channelRepository,
            ITelegramServices telegramConnectorServices)
        {
            _logger = logger;
            _channelRepository = channelRepository;
            _telegramConnectorServices = telegramConnectorServices;
        }

        public async Task AddChannelAsync(string channelName)
        {
            try
            {
                Channel? channel = await _telegramConnectorServices.SearchChannelAsync(channelName);

                if (channel != null) await _channelRepository.AddChannelAsync(channel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Services: {ServicesName} in Method: {MethodName},",
                    nameof(ChannelServices),
                    nameof(AddChannelAsync));

                throw new Exception();
            }
        }

        public Task<bool> DeleteChannelAsync(string channelName)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteChannelByIdAsync(long id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Channel?>> GetAllChannelsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Channel?> GetChannelAsync(string channelName)
        {
            throw new NotImplementedException();
        }

        public Task<Channel?> GetChannelByIdAsync(long id)
        {
            throw new NotImplementedException();
        }
    }
}
