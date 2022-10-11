namespace Fishie.Services.TelegramService.Commands;

/// <summary>
/// Command 
/// </summary>
internal abstract class Command
{
    public long? ChatId { get; set; }
    public string? Action { get; set; }

}
