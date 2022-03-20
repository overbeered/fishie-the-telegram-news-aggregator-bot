using Fishie.Core.Repositories;
using Fishie.Core.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fishie.Core.Models;


namespace Fishie.Services.ChatService
{
    public class ChatServices : IChatServices
    {
        private readonly ILogger<ChatServices> _logger;
        private readonly IChannelRepository _channelRepository;
        private readonly ITelegramServices _telegramServices;
        public ChatServices(ILogger<ChatServices> logger,
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
                    nameof(ChatServices),
                    nameof(AddChannelAsync));
            }
        }

        public async Task DeleteChannelAsync(string channelName)
        {
            try
            {
                await _channelRepository.DeleteChannelAsync(channelName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Services: {ServicesName} in Method: {MethodName},",
                    nameof(ChatServices),
                    nameof(DeleteChannelAsync));
            }
        }

        public async Task DeleteChannelByIdAsync(long id)
        {
            try
            {
                await _channelRepository.DeleteChannelByIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Services: {ServicesName} in Method: {MethodName},",
                    nameof(ChatServices),
                    nameof(DeleteChannelByIdAsync));
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
                    nameof(ChatServices),
                    nameof(GetAllChannelsAsync));
            }

            return null;
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
                    nameof(ChatServices),
                    nameof(GetChannelAsync));
            }

            return null;
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
                    nameof(ChatServices),
                    nameof(GetChannelByIdAsync));
            }

            return null;
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
                    nameof(ChatServices),
                    nameof(SubscribeAsync));
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
                    nameof(ChatServices),
                    nameof(UnsubscribeAsync));
            }
        }

        public async Task<List<string?>?> GetMessagesChannelAsync(string channelName, int count = 5)
        {
            try
            {
                var channel = await _channelRepository.GetChannelAsync(channelName);
                if (channel == null) return null;

                return await _telegramServices.GetMessagesChannelAsync(channel!, count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Services: {ServicesName} in Method: {MethodName},",
                    nameof(ChatServices),
                    nameof(GetMessagesChannelAsync));
            }

            return null;
        }

        public async Task SendMessagesChannelAsync(string channelName, string message)
        {
            try
            {
                var channel = await _channelRepository.GetChannelAsync(channelName);
                if (channel != null) await _telegramServices.SendMessagesChannelAsync(channel!, message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Services: {ServicesName} in Method: {MethodName},",
                    nameof(ChatServices),
                    nameof(SendMessagesChannelAsync));
            }
        }
    }
}
