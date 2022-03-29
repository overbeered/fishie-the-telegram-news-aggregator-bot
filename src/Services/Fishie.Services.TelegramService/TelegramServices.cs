using Fishie.Core.Configuration;
using Fishie.Core.Repositories;
using Fishie.Core.Services;
using Microsoft.Extensions.DependencyInjection;
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
        private readonly AdminConfiguration _adminConfiguration;
        private readonly Client _client;
        private readonly СlientConfiguration _сlientConfiguration;
        private readonly ChatConfiguration _chatConfiguration;

        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly MessagesHandler _messagesHandler;
        public bool Disconnected { get { return _client.Disconnected; } }

        public TelegramServices(ILogger<TelegramServices> logger,
            AdminConfiguration adminConfiguration,
            СlientConfiguration сlientConfiguration,
            ChatConfiguration chatConfiguration,
            IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _adminConfiguration = adminConfiguration;
            _сlientConfiguration = сlientConfiguration;
            _chatConfiguration = chatConfiguration;
            _serviceScopeFactory = serviceScopeFactory;
            _client = new Client(Config);
            _client.Update += OnUpdates;
            _messagesHandler = new MessagesHandler(_serviceScopeFactory);
        }

        public async Task LoginAsync()
        {
            await _client.LoginUserIfNeeded();
            await AddAdminConfiguration(_adminConfiguration);
            await AddChatConfiguration(_chatConfiguration);
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
                        await _messagesHandler!.HandleAsync(_client,
                            m.From != null ? m.From.ID : null,
                            m.Peer.ID,
                            m.ID,
                            m.message);
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

        /// <summary>
        /// Adds the chat configuration to the database
        /// </summary>
        /// <param name="chatConfiguration">Chat configuration</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private async Task AddChatConfiguration(ChatConfiguration chatConfiguration)
        {
            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    IChatRepository chatRepository = scope.ServiceProvider.GetRequiredService<IChatRepository>();

                    var search = await _client.Contacts_Search(chatConfiguration.ChatName);
                    if (search == null) throw new Exception($"channel {search} not found");

                    foreach (var (_, chat) in search.chats)
                    {
                        if (((Channel)chat).username == _chatConfiguration.ChatName || chat.Title == _chatConfiguration.ChatName)
                        {
                            var channel = (InputPeerChannel)chat.ToInputPeer();
                            var coreChannel = new CoreModels.Chat(
                                    channel.channel_id,
                                    ((Channel)chat).username != null ? ((Channel)chat).username : chat.Title,
                                    channel.access_hash);
                            await chatRepository!.AddChatAsync(coreChannel);
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Services: {ServicesName} in Method: {MethodName},",
                    nameof(TelegramServices),
                    nameof(AddChatConfiguration));
            }
        }


        private async Task AddAdminConfiguration(AdminConfiguration adminConfiguration)
        {
            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    IAdminRepository adminRepository = scope.ServiceProvider.GetRequiredService<IAdminRepository>();

                    var search = await _client.Contacts_Search(adminConfiguration.Username);
                    if (search == null) throw new Exception($"Username {search} not found");

                    foreach (var (_, user) in search.users)
                    {
                        if (user.username == adminConfiguration.Username)
                        {
                            var core = new CoreModels.Admin(user.ID, user.first_name, user.last_name, user.username);
                            await adminRepository!.AddAdminAsync(core);
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Services: {ServicesName} in Method: {MethodName},",
                    nameof(TelegramServices),
                    nameof(AddAdminConfiguration));
            }
        }
    }
}
