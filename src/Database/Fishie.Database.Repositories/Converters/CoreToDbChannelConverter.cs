using CoreModels = Fishie.Core.Models;
using DbModels = Fishie.Database.Models;

namespace Fishie.Database.Repositories.Converters
{
    internal static class CoreToDbChannelConverter
    {
        public static DbModels.Channel? Convert(CoreModels.Channel? coreChannel)
        {
            if (coreChannel == null) return null;

            return new DbModels.Channel()
            {
                Id = coreChannel.Id,
                Name = coreChannel.Name,
                AccessHash = coreChannel.AccessHash,
            };
        }

        public static CoreModels.Channel? ConvertBack(DbModels.Channel? dbChannel)
        {
            if (dbChannel == null) return null;

            return new CoreModels.Channel(dbChannel.Id,
                dbChannel.Name!,
                dbChannel.AccessHash);
        }
    }
}
