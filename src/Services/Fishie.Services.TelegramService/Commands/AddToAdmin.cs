using Fishie.Core.Models;
using Fishie.Core.Repositories;
using Fishie.Services.TelegramService.Commands.ResponseCommands;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using TL;
using WTelegram;

namespace Fishie.Services.TelegramService.Commands
{
    /// <summary>
    /// Find and add a admin to the database. Example: /addAdmin username
    /// </summary>
    internal class AddToAdmin : ICommand
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public AddToAdmin(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task ExecuteAsync(Client client, long chatId, string action)
        {
            if (action.IndexOf("--info") != -1)
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    await new ResponseCommand(_serviceScopeFactory).ExecuteAsync(client,
                        chatId,
                        "Find and add a admin to the database. Example: /addAdmin username");
                }
            }
            else
            {
                var search = await client.Contacts_Search(action);

                if (search.users.Count == 0)
                {
                    await new ResponseCommand(_serviceScopeFactory).ExecuteAsync(client,
                        chatId,
                        $"user {action} not found");
                }
                else
                {
                    foreach (var (_, user) in search.users)
                    {
                        if (user.username == action)
                        {
                            using (var scope = _serviceScopeFactory.CreateScope())
                            {
                                IAdminRepository adminRepository = scope.ServiceProvider.GetRequiredService<IAdminRepository>();
                                var admin = new Admin(user.ID, user.first_name, user.last_name, user.username);
                                await adminRepository!.AddAdminAsync(admin);
                            }
                            break;
                        }
                    }
                }
            }
        }
    }
}
