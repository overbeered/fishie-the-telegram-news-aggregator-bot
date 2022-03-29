using Fishie.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fishie.Core.Repositories
{
    public interface IAdminRepository
    {
        Task AddAdminAsync(Admin admin);
        Task<Admin?> GetAdminAsync(string adminName);
        Task DeleteAdminUsernameAsync(string adminUsername);
        Task<bool> AdminByIdExistsAsync(long id);
        Task<IEnumerable<Admin?>?> GetAllAdminAsync();
    }
}
