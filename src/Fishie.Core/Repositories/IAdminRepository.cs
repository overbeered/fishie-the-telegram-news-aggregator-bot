using Fishie.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fishie.Core.Repositories
{
    /// <summary>
    ///  Defines Admin repository interface.
    /// </summary>
    public interface IAdminRepository
    {
        /// <summary>
        /// Adds a channel
        /// </summary>
        /// <param name="admin">Admin</param>
        /// <exception>The admin is already stored in the database</exception>
        Task AddAdminAsync(Admin admin);

        /// <summary>
        ///  Returns the Admin by first name 
        /// </summary>
        /// <param name="firstName">First name </param>
        /// <returns>If it finds it, it will return - Admin, if not - null</returns>
        Task<Admin?> FindAdminAsync(string firstName);

        /// <summary>
        ///  Deletes a admin by username
        /// </summary>
        /// <param name="adminUsername">Admin username</param>
        /// <exception>Does not find a admin with this username in the database</exception>
        Task DeleteAdminUsernameAsync(string adminUsername);

        /// <summary>
        /// Checking availability admin
        /// </summary>
        /// <param name="id">Admin id</param>
        /// <returns>Returns true if at least one element of the database is defined by the condition</returns>
        Task<bool> AdminByIdExistsAsync(long id);

        /// <summary>
        /// Returns admin
        /// </summary>
        /// <returns>If it finds it, it will return a Admin list, if not, a null list</returns>
        Task<List<Admin?>> FindAllAdminAsync();
    }
}
