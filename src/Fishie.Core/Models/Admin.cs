namespace Fishie.Core.Models;

/// <summary>
/// Telegram admin model for chat
/// </summary>
public class Admin
{
    /// <summary>
    /// Telegram user Id
    /// </summary>
    public long Id { get; private set; }

    /// <summary>
    /// Telegram user first name
    /// </summary>
    public string? FirstName { get; private set; }

    /// <summary>
    /// Telegram user last name
    /// </summary>
    public string? LastName { get; private set; }

    /// <summary>
    /// Telegram username
    /// </summary>
    public string? Username { get; private set; }

    public Admin(long id, string? firstName, string? lastName, string? username)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Username = username;
    }
}