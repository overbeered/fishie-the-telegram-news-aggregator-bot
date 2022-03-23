using System.Threading.Tasks;
using WTelegram;

namespace Fishie.Services.TelegramService
{
    internal interface ICommand
    {
        Task ExecuteAsync(Client client, string action);
    }
}
