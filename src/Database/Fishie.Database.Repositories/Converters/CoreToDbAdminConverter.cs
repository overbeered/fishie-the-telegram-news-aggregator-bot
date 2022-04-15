using CoreModels = Fishie.Core.Models;
using DbModels = Fishie.Database.Models;

namespace Fishie.Database.Repositories.Converters
{
    internal static class CoreToDbAdminConverter
    {
        public static DbModels.Admin? Convert(CoreModels.Admin? core)
        {
            if (core == null) return null;

            return new DbModels.Admin(core.Id,
                core.FirstName,
                core.LastName,
                core.Username);
        }

        public static CoreModels.Admin? ConvertBack(DbModels.Admin? db)
        {
            if (db == null) return null;

            return new CoreModels.Admin(db.Id,
                db.FirstName,
                db.LastName,
                db.Username);
        }
    }
}
