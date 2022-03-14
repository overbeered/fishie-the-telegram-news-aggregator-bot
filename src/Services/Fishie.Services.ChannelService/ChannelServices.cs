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
        private readonly ITelegramServices _telegramServices;
        public ChannelServices(ILogger<ChannelServices> logger,
            IChannelRepository channelRepository,
            ITelegramServices telegramServices)
        {
            _logger = logger;
            _channelRepository = channelRepository;
            _telegramServices = telegramServices;
        }

        public async Task AddChannelAsync(string channelName)
        {
            try
            {
                Channel? channel = await _telegramServices.SearchChannelAsync(channelName);

                if (channel == null) throw new Exception($"channel {channelName} not found");

                await _channelRepository.AddChannelAsync(channel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Services: {ServicesName} in Method: {MethodName},",
                    nameof(ChannelServices),
                    nameof(AddChannelAsync));

                throw new Exception();
            }
        }

        public async Task<bool> DeleteChannelAsync(string channelName)
        {
            try
            {
                return await _channelRepository.DeleteChannelAsync(channelName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Services: {ServicesName} in Method: {MethodName},",
                    nameof(ChannelServices),
                    nameof(DeleteChannelAsync));

                throw new Exception();
            }
        }

        public async Task<bool> DeleteChannelByIdAsync(long id)
        {
            try
            {
                return await _channelRepository.DeleteChannelByIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Services: {ServicesName} in Method: {MethodName},",
                    nameof(ChannelServices),
                    nameof(DeleteChannelByIdAsync));

                throw new Exception();
            }
        }

        public async Task<IEnumerable<Channel?>?> GetAllChannelsAsync()
        {
            try
            {
                return await _channelRepository.GetAllChannelsAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Services: {ServicesName} in Method: {MethodName},",
                    nameof(ChannelServices),
                    nameof(GetAllChannelsAsync));

                throw new Exception();
            }
        }

        public async Task<Channel?> GetChannelAsync(string channelName)
        {
            try
            {
                return await _channelRepository.GetChannelAsync(channelName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Services: {ServicesName} in Method: {MethodName},",
                    nameof(ChannelServices),
                    nameof(GetChannelAsync));

                throw new Exception();
            }
        }

        public async Task<Channel?> GetChannelByIdAsync(long id)
        {
            try
            {
                return await _channelRepository.GetChannelByIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Services: {ServicesName} in Method: {MethodName},",
                    nameof(ChannelServices),
                    nameof(GetChannelByIdAsync));

                throw new Exception();
            }
        }

        public async Task SubscribeAsync(string channelName)
        {
            try
            {
                var channel =  await _channelRepository.GetChannelAsync(channelName);
                if (channel != null) await _telegramServices.SubscribeAsync(channel!);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Services: {ServicesName} in Method: {MethodName},",
                    nameof(ChannelServices),
                    nameof(SubscribeAsync));

                throw new Exception();
            }
        }

        public async Task UnsubscribeAsync(string channelName)
        {
            try
            {
                var channel = await _channelRepository.GetChannelAsync(channelName);
                if (channel != null) await _telegramServices.UnsubscribeAsync(channel!);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Services: {ServicesName} in Method: {MethodName},",
                    nameof(ChannelServices),
                    nameof(UnsubscribeAsync));

                throw new Exception();
            }
        }
    }
}
