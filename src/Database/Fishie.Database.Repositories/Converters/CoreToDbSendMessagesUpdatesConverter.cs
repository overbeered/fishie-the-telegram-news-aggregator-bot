using CoreModels = Fishie.Core.Models;
using DbModels = Fishie.Database.Models;

namespace Fishie.Database.Repositories.Converters
{
    internal static class CoreToDbSendMessagesUpdatesConverter
    {
        public static DbModels.ForwardMessages? Convert(CoreModels.ForwardMessages? core)
        {
            if (core == null) return null;

            return new DbModels.ForwardMessages(core.ChannelId, core.ChatId);
        }

        public static CoreModels.ForwardMessages? ConvertBack(DbModels.ForwardMessages? db)
        {
            if (db == null) return null;

            return new CoreModels.ForwardMessages(db.ChannelId, db.ChatId);
        }
    }
}
