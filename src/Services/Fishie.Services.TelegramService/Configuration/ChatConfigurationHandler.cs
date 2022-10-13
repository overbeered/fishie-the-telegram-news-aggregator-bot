using Fishie.Core.Configuration;
using Fishie.Core.Repositories;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TL;
using WTelegram;
using CoreModels = Fishie.Core.Models;

namespace Fishie.Services.TelegramService.Configuration;

/// <summary>
/// Chat configuration, search and adding to the database
/// </summary>
internal class ChatConfigurationHandler : INotificationHandler<ConfigurationNotification>
{
    private readonly ILogger<ChatConfigurationHandler> _logger;
    private readonly ChatConfiguration _chatConfiguration;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly Client _client;

    public ChatConfigurationHandler(ILogger<ChatConfigurationHandler> logger,
        ChatConfiguration chatConfiguration,
        IServiceScopeFactory serviceScopeFactory,
        Client client)
    {
        _logger = logger;
        _chatConfiguration = chatConfiguration;
        _serviceScopeFactory = serviceScopeFactory;
        _client = client;
    }

    public async Task Handle(ConfigurationNotification notification, CancellationToken cancellationToken)
    {
        try
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var chatRepository = scope.ServiceProvider.GetRequiredService<IChatRepository>();

            var search = await _client.Contacts_Search(_chatConfiguration.ChatName);
           
            if (search == null) throw new Exception($"channel {search} not found");

            foreach (var (_, chat) in search.chats)
            {
                if (((Channel)chat).username == _chatConfiguration.ChatName || chat.Title == _chatConfiguration.ChatName)
                {
                    var channel = (InputPeerChannel)chat.ToInputPeer();
                    var coreChannel = new CoreModels.Chat(
                            channel.channel_id,
                            channel.access_hash,
                            chat.Title,
                            ((Channel)chat).username);

                    if (!await chatRepository.ExistsAsync(coreChannel))
                    {
                        await chatRepository.AddAsync(coreChannel);
                        break;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in Class: {ServicesName}:,",
                nameof(ChatConfigurationHandler));
        }
    }
}