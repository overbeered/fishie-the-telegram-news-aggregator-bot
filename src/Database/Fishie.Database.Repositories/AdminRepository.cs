using Fishie.Core.Repositories;
using Fishie.Database.Context;
using Fishie.Database.Repositories.Converters;
using Microsoft.EntityFrameworkCore;
using CoreModels = Fishie.Core.Models;
using DbModels = Fishie.Database.Models;

namespace Fishie.Database.Repositories;

public class AdminRepository : IAdminRepository
{
    private readonly NpgSqlContext _dbContext;

    public AdminRepository(NpgSqlContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(CoreModels.Admin admin)
    {
        await _dbContext.AddAsync(CoreToDbAdminConverter.Convert(admin)!);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(long id)
    {
        return await _dbContext.Admins.AsNoTracking().AnyAsync(a => a.Id == id);
    }

    public async Task DeleteUsernameAsync(string username)
    {
        DbModels.Admin admin = await _dbContext.Admins.FirstAsync(a => a.Username == username);
        _dbContext.Admins.Remove(admin);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<CoreModels.Admin?> FindAsync(string username)
    {
        DbModels.Admin? admin = await _dbContext.Admins.AsNoTracking()
            .FirstOrDefaultAsync(a => a.Username == username);

        return CoreToDbAdminConverter.ConvertBack(admin);
    }

    public async Task<List<CoreModels.Admin?>> FindAllAsync()
    {
        return await _dbContext.Admins.AsNoTracking()
            .Select(data => CoreToDbAdminConverter.ConvertBack(data))
            .ToListAsync();
    }
}