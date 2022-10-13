namespace Fishie.Services.TelegramService.Commands;

/// <summary>
/// Command 
/// </summary>
internal abstract class Command
{
    /// <summary>
    /// Chat id
    /// </summary>
    public long? ChatId { get; set; }

    /// <summary>
    /// Action
    /// </summary>
    public string? Action { get; set; }
}
