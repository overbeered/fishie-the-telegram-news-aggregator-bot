using System.Threading.Tasks;

namespace Fishie.Core.Services
{
    /// <summary>
    /// Service for the commands
    /// </summary>
    public interface IMessagesHandler
    {
        /// <summary>
        /// Applies the command
        /// </summary>
        /// <param name="command">Command</param>
        /// <returns></returns>
        Task Handle(string command);
    }
}
