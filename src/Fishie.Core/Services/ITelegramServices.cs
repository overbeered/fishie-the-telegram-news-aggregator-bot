namespace Fishie.Core.Services;

/// <summary>
/// Telegram services
/// </summary>
public interface ITelegramServices
{
    /// <summary>
    /// Login as a user (if not already logged-in).
    /// </summary>
    Task LoginAsync();

    /// <summary>
    /// Disconnect from Telegram 
    /// </summary>
    void Reset();

    /// <summary>
    /// Has this Client established connection been disconnected?
    /// </summary>
    bool Disconnected { get; }
}