namespace Fishie.Server.Configuration;

internal class ConfigTelegram
{
    /// <summary>
    /// Сonfiguration of the application to connect to telegram
    /// </summary>
    /// <param name="what"></param>
    /// <returns></returns>
    public static string? Config(string what)
    {
        switch (what)
        {
            case "api_id": return "";
            case "api_hash": return "";
            case "phone_number": return "";
            case "verification_code": Console.Write("Code: "); return Console.ReadLine()!;
            case "first_name": return "Overbeered";
            case "last_name": return "Overbeered";
            case "password": return "";
            default: return null;
        }
    }
}