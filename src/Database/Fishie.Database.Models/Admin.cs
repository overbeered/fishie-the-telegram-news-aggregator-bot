namespace Fishie.Database.Models;

public class Admin
{
    public long Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
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