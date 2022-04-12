using Fishie.Core.Configuration;
using Fishie.Core.Services;
using Fishie.Services.TelegramService.Configuration;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using TL;
using WTelegram;

namespace Fishie.Services.TelegramService
{
    public class TelegramServices : ITelegramServices
    {
        private readonly ILogger<TelegramServices> _logger;
        private readonly Client _client;
        private readonly СlientConfiguration _сlientConfiguration;
        private readonly IMediator _mediator;
        public bool Disconnected { get { return _client.Disconnected; } }

        public TelegramServices(ILogger<TelegramServices> logger,
            СlientConfiguration сlientConfiguration,
            IMediator mediator)
        {
            _logger = logger;
            _сlientConfiguration = сlientConfiguration;
            _mediator = mediator;
            _client = new Client(Config);
            _client.Update += OnUpdates;
        }

        public async Task LoginAsync()
        {
            await _client.LoginUserIfNeeded();
            await _mediator.Publish(new ConfigurationNotification() { Client = _client });
        }

        public void Reset()
        {
            _client.Reset();
        }

        /// <summary>
        /// Updating events
        /// </summary>
        /// <param name="obj"></param>
        private async void OnUpdates(IObject obj)
        {
            if (obj is not UpdatesBase updates) return;
            foreach (var update in updates.UpdateList)
                switch (update)
                {
                    case UpdateNewMessage unm: await DisplayMessage(unm.message); break;
                }
        }

        /// <summary>
        /// Processes messages
        /// </summary>
        /// <param name="messageBase"></param>
        /// <returns></returns>
        private async Task DisplayMessage(MessageBase messageBase)
        {
            try
            {
                switch (messageBase)
                {
                    case Message m:
                        await _mediator.Send(new MessagesRequest()
                        {
                            Client = _client,
                            UserId = m.From != null ? m.From.ID : null,
                            ChatId = m.Peer.ID,
                            MessageId = m.ID,
                            Message = m.message,
                        });
                        break;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Services: {ServicesName} in Method: {MethodName},",
                    nameof(TelegramServices),
                    nameof(DisplayMessage));
            }
        }

        /// <summary>
        /// Сonfiguration of the application to connect to telegram
        /// </summary>
        /// <param name="what"></param>
        /// <returns></returns>
        private string? Config(string what)
        {
            switch (what)
            {
                case "api_id": return _сlientConfiguration.ApiId;
                case "api_hash": return _сlientConfiguration.ApiHash;
                case "phone_number": return _сlientConfiguration.PhoneNumber;
                case "verification_code": Console.Write("Code: "); return Console.ReadLine()!;
                case "first_name": return _сlientConfiguration.FirstName;
                case "last_name": return _сlientConfiguration.LastName;
                case "password": return _сlientConfiguration.Password;
                default: return null;
            }
        }
    }
}
