using Fishie.Core.Connectors;
using Fishie.Core.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using TL;
using WTelegram;
using CoreModels = Fishie.Core.Models;

namespace Fishie.Services.TelegramService
{
    public class TelegramServices : ITelegramServices
    {
        private readonly ILogger<TelegramServices> _logger;
        private readonly СlientConnector _сlientConnector;
        private Client _client;

        public TelegramServices(ILogger<TelegramServices> logger,
            СlientConnector сlientConnector)
        {
            _logger = logger;
            _сlientConnector = сlientConnector;
            _client = new Client(Config);
            // make asynchronous
            _client.LoginUserIfNeeded();
        }


        public async Task<CoreModels.Channel?> SearchChannelAsync(string query)
        {
            try
            {
                var search = await _client.Contacts_Search(query);

                foreach (var (id, chat) in search.chats)
                {
                    if (chat.Title == query && chat.IsActive)
                    {
                        var channel = (InputPeerChannel)chat.ToInputPeer();

                        return new CoreModels.Channel(
                                channel.channel_id,
                                chat.Title,
                                channel.access_hash);
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Services: {ServicesName} in Method: {MethodName},",
                    nameof(TelegramServices),
                    nameof(SearchChannelAsync));

                throw new Exception();
            }

        }

        public async Task SubscribeAsync(CoreModels.Channel channel)
        {
            try
            {
                await _client.Channels_JoinChannel(new InputChannel()
                {
                    channel_id = channel.Id,
                    access_hash = channel.AccessHash
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Services: {ServicesName} in Method: {MethodName},",
                    nameof(TelegramServices),
                    nameof(SubscribeAsync));

                throw new Exception();
            }
        }

        public async Task UnsubscribeAsync(CoreModels.Channel channel)
        {
            try
            {
                await _client.LeaveChat(new InputChannel()
                {
                    channel_id = channel.Id,
                    access_hash = channel.AccessHash
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Services: {ServicesName} in Method: {MethodName},",
                    nameof(TelegramServices),
                    nameof(UnsubscribeAsync));

                throw new Exception();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="what"></param>
        /// <returns></returns>
        private string Config(string what)
        {
            switch (what)
            {
                case "api_id": return _сlientConnector.ApiId;
                case "api_hash": return _сlientConnector.ApiHash;
                case "phone_number": return _сlientConnector.PhoneNumber;
                case "verification_code": Console.Write("Code: "); return Console.ReadLine()!;
                case "first_name": return _сlientConnector.FirstName;      
                case "last_name": return _сlientConnector.LastName;        
                case "password": return _сlientConnector.Password;     
                default: return null;
            }
        }

    }
}
