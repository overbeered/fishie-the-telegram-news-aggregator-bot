using CoreModels = Fishie.Core.Models;
using DbModels = Fishie.Database.Models;


namespace Fishie.Database.Repositories.Converters
{
    internal static class CoreToDbChatConverter
    {
        public static DbModels.Chat? Convert(CoreModels.Chat? coreChat)
        {
            if (coreChat == null) return null;

            return new DbModels.Chat(coreChat.Id,
                coreChat.AccessHash,
                coreChat.Name,
                coreChat.Username);
        }

        public static CoreModels.Chat? ConvertBack(DbModels.Chat? dbChat)
        {
            if (dbChat == null) return null;

            return new CoreModels.Chat(dbChat.Id,
                dbChat.AccessHash,
                dbChat.Name,
                dbChat.Username);
        }
    }
}

