using System.Threading.Tasks;
using WTelegram;

namespace Fishie.Services.TelegramService
{
    /// <summary>
    /// Command interface
    /// </summary>
    internal interface ICommand
    {
        Task ExecuteAsync(Client client, long chatId, string action);
    }
}
