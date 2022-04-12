using Fishie.Core.Models;
using Fishie.Core.Repositories;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using System.Threading.Tasks;
using TL;

namespace Fishie.Services.TelegramService.Commands.AddAdmin
{
    /// <summary>
    /// Find and add a admin to the database. Example: /addAdmin username
    /// </summary>
    internal class AddAdminCommandHandle : AsyncRequestHandler<AddAdminCommand>
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public AddAdminCommandHandle(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task Handle(AddAdminCommand request, CancellationToken cancellationToken)
        {
            if (request.Action!.IndexOf("--info") != -1)
            {
                await ResponseCommand.ExecuteAsync(_serviceScopeFactory,
                    request.Client!,
                   (long)(request.ChatId!),
                    "Find and add a admin to the database. Example: /addAdmin username");
            }
            else
            {
                var search = await request.Client.Contacts_Search(request.Action);

                if (search.users.Count == 0)
                {
                    await ResponseCommand.ExecuteAsync(_serviceScopeFactory,
                        request.Client!,
                       (long)(request.ChatId!),
                        $"user {request.Action} not found");
                }
                else
                {
                    foreach (var (_, user) in search.users)
                    {
                        if (user.username == request.Action)
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
