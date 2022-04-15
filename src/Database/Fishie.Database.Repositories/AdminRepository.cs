using Fishie.Core.Repositories;
using Fishie.Database.Context;
using Fishie.Database.Repositories.Converters;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreModels = Fishie.Core.Models;
using DbModels = Fishie.Database.Models;

namespace Fishie.Database.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly NpgSqlContext _dbContext;

        public AdminRepository(NpgSqlContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAdminAsync(CoreModels.Admin admin)
        {
            await _dbContext.AddAsync(CoreToDbAdminConverter.Convert(admin)!);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> AdminByIdExistsAsync(long id)
        {
            return await _dbContext.Admins.AnyAsync(a => a.Id == id);
        }

        public async Task DeleteAdminUsernameAsync(string adminUsername)
        {
            DbModels.Admin admin = await _dbContext.Admins.FirstAsync(a => a.Username == adminUsername);
            _dbContext.Admins.Remove(admin);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<CoreModels.Admin?> FindAdminAsync(string firstName)
        {
            DbModels.Admin? admin = await _dbContext.Admins.FirstOrDefaultAsync(a => a.FirstName == firstName);
            return CoreToDbAdminConverter.ConvertBack(admin);
        }

        public async Task<List<CoreModels.Admin?>> FindAllAdminAsync()
        {
            return await _dbContext.Admins.Select(data => CoreToDbAdminConverter.ConvertBack(data)).ToListAsync();
        }
    }
}
