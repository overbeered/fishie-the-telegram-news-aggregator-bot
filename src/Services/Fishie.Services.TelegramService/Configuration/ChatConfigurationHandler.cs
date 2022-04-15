using Fishie.Core.Configuration;
using Fishie.Core.Repositories;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using TL;
using CoreModels = Fishie.Core.Models;

namespace Fishie.Services.TelegramService.Configuration
{
    internal class ChatConfigurationHandler : INotificationHandler<ConfigurationNotification>
    {
        private readonly ILogger<ChatConfigurationHandler> _logger;
        private readonly ChatConfiguration _chatConfiguration;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public ChatConfigurationHandler(ILogger<ChatConfigurationHandler> logger,
            ChatConfiguration chatConfiguration,
            IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _chatConfiguration = chatConfiguration;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task Handle(ConfigurationNotification notification, CancellationToken cancellationToken)
        {
            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    IChatRepository chatRepository = scope.ServiceProvider.GetRequiredService<IChatRepository>();

                    var search = await notification.Client.Contacts_Search(_chatConfiguration.ChatName);
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

                            if (!await chatRepository.ChatByIdExistsAsync(coreChannel))
                            {
                                await chatRepository.AddChatAsync(coreChannel);
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Class: {ConfigurationNotification}:,",
                    nameof(TelegramServices));
            }
        }
    }
}
