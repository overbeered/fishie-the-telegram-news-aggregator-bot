using Fishie.Core.Repositories;
using Fishie.Database.Context;
using Fishie.Database.Repositories.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
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
        private readonly ILogger<AdminRepository> _logger;

        public AdminRepository(NpgSqlContext dbContext,
            ILogger<AdminRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task AddAdminAsync(CoreModels.Admin admin)
        {
            try
            {
                var isAdminExists = await _dbContext.Admins!.AnyAsync(a => a.Id == admin.Id);

                if (!isAdminExists)
                {
                    await _dbContext.AddAsync(CoreToDbAdminConverter.Convert(admin)!);
                    await _dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Repository: {RepositoryName} in Method: {MethodName},",
                    nameof(AdminRepository),
                    nameof(AddAdminAsync));
            }
        }

        public async Task<bool> AdminByIdExistsAsync(long id)
        {
            try
            { 
                return await _dbContext.Admins!.AnyAsync(a => a.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Repository: {RepositoryName} in Method: {MethodName},",
                    nameof(AdminRepository),
                    nameof(AdminByIdExistsAsync));
            }

            return false;
        }

        public async Task DeleteAdminUsernameAsync(string adminUsername)
        {
            try
            {
                DbModels.Admin? admin = await _dbContext.Admins!.FirstOrDefaultAsync(a => a.Username == adminUsername);

                if (admin != null)
                {
                    _dbContext.Admins!.Remove(admin);
                    await _dbContext.SaveChangesAsync();
                }
                else
                {
                    throw new Exception($"Admin name - {admin} is not found");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Repository: {RepositoryName} in Method: {MethodName},",
                    nameof(ChannelRepository),
                    nameof(DeleteAdminUsernameAsync));
            }
        }

        public async Task<CoreModels.Admin?> GetAdminAsync(string adminName)
        {
            try
            {
                DbModels.Admin? admin = await _dbContext.Admins!.FirstOrDefaultAsync(a => a.FirstName == adminName);

                return CoreToDbAdminConverter.ConvertBack(admin);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Repository: {RepositoryName} in Method: {MethodName},",
                    nameof(ChannelRepository),
                    nameof(GetAdminAsync));
            }

            return null;
        }

        public async Task<IEnumerable<CoreModels.Admin?>?> GetAllAdminAsync()
        {
            try
            {
                IEnumerable<DbModels.Admin?> stored = await _dbContext.Admins!.ToListAsync();

                return stored.Select(data => CoreToDbAdminConverter.ConvertBack(data));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Repository: {RepositoryName} in Method: {MethodName},",
                    nameof(ChannelRepository),
                    nameof(GetAllAdminAsync));
            }

            return null;
        }
    }
}
