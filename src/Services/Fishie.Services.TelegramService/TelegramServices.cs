using Fishie.Core.Configurat;
using Fishie.Core.Repositories;
using Fishie.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using TL;
using WTelegram;

namespace Fishie.Services.TelegramService
{
    public class TelegramServices : ITelegramServices
    {
        private readonly ILogger<TelegramServices> _logger;
        private readonly Client _client;
        private readonly СlientConfigurat _сlientConfigurat;
        private readonly ChatConfigurat _chatConfigurat;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public bool Disconnected { get { return _client.Disconnected; } }

        public TelegramServices(ILogger<TelegramServices> logger,
            СlientConfigurat сlientConfigurat,
            ChatConfigurat chatConfigurat,
            IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _сlientConfigurat = сlientConfigurat;
            _chatConfigurat = chatConfigurat;
            _serviceScopeFactory = serviceScopeFactory;
            _client = new Client(Config);
            _client.Update += OnUpdates;
        }

        private async void OnUpdates(IObject obj)
        {
            if (obj is not UpdatesBase updates) return;

            foreach (var update in updates.UpdateList)
                switch (update)
                {
                    case UpdateNewMessage unm: await DisplayMessage(unm.message); break;
                }
        }

        private async Task DisplayMessage(MessageBase messageBase)
        {
            try
            {
                MessagesHandler messagesHandler = new MessagesHandler(_serviceScopeFactory);
                //Think 
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    IChatRepository chatRepository = scope.ServiceProvider.GetRequiredService<IChatRepository>();
                    var listChats = await chatRepository.GetAllChatsAsync();

                    if (listChats?.Any() != true)
                    {
                        await messagesHandler.Handle(_client, "/addChat " + _chatConfigurat.ChatName);
                    }

                    foreach (var chat in listChats!)
                    {
                        if (messageBase.Peer.ID == chat!.Id)
                        {
                            switch (messageBase)
                            {
                                case Message m:
                                    await messagesHandler.Handle(_client, m.message);
                                    break;
                            }
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Services: {ServicesName} in Method: {MethodName},",
                    nameof(TelegramServices),
                    nameof(DisplayMessage));
            }
        }

        public async Task LoginAsync()
        {
            await _client.LoginUserIfNeeded();
        }

        public void Reset()
        {
            _client.Reset();
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
                case "api_id": return _сlientConfigurat.ApiId;
                case "api_hash": return _сlientConfigurat.ApiHash;
                case "phone_number": return _сlientConfigurat.PhoneNumber;
                case "verification_code": Console.Write("Code: "); return Console.ReadLine()!;
                case "first_name": return _сlientConfigurat.FirstName;
                case "last_name": return _сlientConfigurat.LastName;
                case "password": return _сlientConfigurat.Password;
                default: return null;
            }
        }
    }
}
