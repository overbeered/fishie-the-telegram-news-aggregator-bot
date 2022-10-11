using Fishie.Core.Configuration;
using Fishie.Core.Models;
using Fishie.Core.Repositories;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TL;
using WTelegram;

namespace Fishie.Services.TelegramService.Configuration;

internal class AdminConfigurationHandler : INotificationHandler<ConfigurationNotification>
{
    private readonly ILogger<AdminConfigurationHandler> _logger;
    private readonly AdminConfiguration _adminConfiguration;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly Client _client;
    public AdminConfigurationHandler(ILogger<AdminConfigurationHandler> logger,
        AdminConfiguration adminConfiguration,
        IServiceScopeFactory serviceScopeFactory,
        Client client)
    {
        _logger = logger;
        _adminConfiguration = adminConfiguration;
        _serviceScopeFactory = serviceScopeFactory;
        _client = client;
    }

    public async Task Handle(ConfigurationNotification notification, CancellationToken cancellationToken)
    {
        try
        {
            using var scope = _serviceScopeFactory.CreateScope();
            IAdminRepository adminRepository = scope.ServiceProvider.GetRequiredService<IAdminRepository>();

            var search = await _client.Contacts_Search(_adminConfiguration.Username);
            if (search == null) throw new Exception($"Username {search} not found");

            foreach (var (_, user) in search.users)
            {
                if (user.username == _adminConfiguration.Username)
                {
                    var core = new Admin(user.ID, user.first_name, user.last_name, user.username);

                    if (!await adminRepository.ExistsAsync(core.Id))
                    {
                        await adminRepository.AddAsync(core);
                        break;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in Class: {ServicesName}",
                nameof(AdminConfigurationHandler));
        }
    }
}