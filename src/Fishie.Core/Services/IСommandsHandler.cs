using System.Threading.Tasks;

namespace Fishie.Core.Services
{
    public interface IСommandsHandler
    {
        Task HandleAsync(string command);
    }
}
