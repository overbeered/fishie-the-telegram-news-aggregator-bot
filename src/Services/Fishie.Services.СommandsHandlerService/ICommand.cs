namespace Fishie.Services.СommandsHandlerService
{
    internal interface ICommand
    {
        Task ExecuteAsync(string action);
    }
}
