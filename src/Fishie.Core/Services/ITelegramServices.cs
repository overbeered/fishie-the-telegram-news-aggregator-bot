using Fishie.Core.Models;
using System.Threading.Tasks;

namespace Fishie.Core.Services
{
    public interface ITelegramServices
    {
        Task<Channel?> SearchChannelAsync(string query);
    }
}
