using Fishie.Core.Connectors;
using Fishie.Core.Services;
using System;
using WTelegram;
using System.Threading.Tasks;
using TL;
using CoreModels = Fishie.Core.Models;
using Microsoft.Extensions.Logging;

namespace Fishie.Services.ConnectorService
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="what"></param>
        /// <returns></returns>
        private string? Config(string what)
        {
            switch (what)
            {
                case "api_id": return _сlientConnector.ApiId;
                case "api_hash": return _сlientConnector.ApiHash;
                case "phone_number": return _сlientConnector.PhoneNumber;
                case "verification_code": Console.Write("Code: "); return Console.ReadLine()!;
                case "first_name": return _сlientConnector.FirstName;      // if sign-up is required
                case "last_name": return _сlientConnector.LastName;        // if sign-up is required
                case "password": return _сlientConnector.Password;     // if user has enabled 2FA
                default: return null;                  // let WTelegramClient decide the default config
            }
        }

    }
}
