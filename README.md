# tg-news-aggregator-bot

The application uses the client library [WTelegramClient](https://github.com/wiz0u/WTelegramClient). The main essence of the project is that it can forward relevant posts from channels to chats.

## Configuration telegram

Default configuration file **..\src\Fishie.Server\appsettings.json**

Configuration of the application to connect to telegram
```json
"api_id": "123",
"api_hash": "123",
"phone_number": "+78005553535",
"first_name": "Overbeered",
"last_name": "Overbeered",
"password": "Overbeered",
```

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