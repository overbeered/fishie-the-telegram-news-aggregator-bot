using CoreModels = Fishie.Core.Models;
using DbModels = Fishie.Database.Models;

namespace Fishie.Database.Repositories.Converters;

internal static class CoreToDbChannelConverter
{
    public static DbModels.Channel? Convert(CoreModels.Channel? coreChannel)
    {
        if (coreChannel == null) return null;

        return new DbModels.Channel(coreChannel.Id,
            coreChannel.AccessHash,
            coreChannel.Name,
            coreChannel.Username);
    }

    public static CoreModels.Channel? ConvertBack(DbModels.Channel? dbChannel)
    {
        if (dbChannel == null) return null;

        return new CoreModels.Channel(dbChannel.Id,
            dbChannel.AccessHash,
            dbChannel.Name,
            dbChannel.Username);
    }
}