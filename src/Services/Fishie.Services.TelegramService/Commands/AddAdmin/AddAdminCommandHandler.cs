using Fishie.Core.Models;
using Fishie.Core.Repositories;
using Fishie.Services.TelegramService.Commands.Utils;
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
    internal class AddAdminCommandHandler : AsyncRequestHandler<AddAdminCommand>
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public AddAdminCommandHandler(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task Handle(AddAdminCommand request, CancellationToken cancellationToken)
        {
            string? answer = null;

            if (request.Action!.IndexOf("--info") != -1)
            {
                answer = "Find and add a admin to the database. Example: /addAdmin username";
            }
            else
            {
                var search = await request.Client.Contacts_Search(request.Action);

                if (search.users.Count == 0)
                {
                    answer = $"User {request.Action} not found";
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

                                answer = $"The user {request.Action} has already been added to the database";

                                if (!await adminRepository.AdminByIdExistsAsync(admin.Id))
                                {
                                    await adminRepository!.AddAdminAsync(admin);
                                    answer = $"The user {request.Action} has been added to the database";
                                }
                            }
                            break;
                        }
                    }
                }
            }

            if (answer != null) await CommandResponseHelper.ExecuteAsync(_serviceScopeFactory, request.Client!,
                (long)(request.ChatId!),
                answer);
        }
    }
}
