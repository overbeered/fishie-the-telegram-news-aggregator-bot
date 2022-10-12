namespace Fishie.Database.Models;

/// <summary>
/// Telegram database administrator model for chat
/// </summary>
public class Admin
{
    /// <summary>
    /// Telegram user Id
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Telegram user first name
    /// </summary>
    public string? FirstName { get; set; }

    /// <summary>
    /// Telegram user last name
    /// </summary>
    public string? LastName { get; set; }


    /// <summary>
    /// Telegram username
    /// </summary>
    public string? Username { get; set; }

#nullable disable
    public Admin() { }
#nullable restore

    public Admin(long id,
        string? firstName,
        string? lastName,
        string? username)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Username = username;
    }
}