# tg-news-aggregator-bot

The application uses the client library [WTelegramClient](https://github.com/wiz0u/WTelegramClient). The main essence of the project is that it can forward relevant posts from channels to chats.

## Configuration telegram

Configuration of the application to connect to telegram file **..\src\Fishie.Server\Configuration\ConfigTelegram.cs**

```C#
    public static string? Config(string what)
    {
        switch (what)
        {
            case "api_id": return "123";
            case "api_hash": return "123";
            case "phone_number": return "";
            case "verification_code": Console.Write("Code: "); return Console.ReadLine()!;
            case "first_name": return "Overbeered";
            case "last_name": return "Overbeered";
            case "password": return "";
            default: return null;
        }
    }
```
Default configuration file **..\src\Fishie.Server\appsettings.json**

Default chat configuration for command processing
```json
  "ChatConfiguration": {
    "ChatName": "ChatName"
  }
```

Configuration of the default administrator who can send commands
```json
  "AdminConfiguration": {
    "Username": "Admin"
  }
```

# How teams work?

**/[command] [information]**

**[command]** - bot command

**[information]** - information for the command, if needed

Description of the command

**/[command] --info**

### **List of commands:**
* commands
* addAdmin
* addChannel
* addChat
* deleteChannel
* deleteChannelForward
* getAllChannels
* getAllForward
* subscribe
* unsubscribe
* sendHistory
* sendHistoryWords
* forward

# Example of using a bot

Add a channel to the database

/addChannel Test

Subscribe to a channel Test from the database 

/subscribe Test

Receive new messages from the channel Test

/forward Test