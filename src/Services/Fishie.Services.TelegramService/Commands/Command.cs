using WTelegram;

namespace Fishie.Services.TelegramService.Commands
{
    /// <summary>
    /// Command 
    /// </summary>
    internal abstract class Command
    {
        public Client? Client { get; set; }
        public long? ChatId { get; set; }
        public string? Action { get; set; }

        //подумать 

        //public static abstract string CommandText { get; }
    }
}
