using ApuDoingStuff.Commands;
using ApuDoingStuff.Twitch;
using TwitchLib.Client.Models;

namespace ApuDoingStuff.Messages
{
    public static class MessageCommands
    {

        public static void Laurin(TwitchBot twitchBot, ChatMessage chatMessage)
        {
            if (chatMessage.Username == "lauriin" && !BotAction.IsOnMessageCooldown(chatMessage.Username, MessageType.Laurin))
            {
                BotAction.AddUserToMessageCooldownDictionary(chatMessage.Username, MessageType.Laurin);
                twitchBot.Send(chatMessage.Channel, "Laurin hat nen kleinen Schniedel AlienPls");
                BotAction.AddMessageCooldown(chatMessage.Username, MessageType.Laurin);

            }
        }

        public static void ApuSquats(TwitchBot twitchBot, ChatMessage chatMessage)
        {

            if (chatMessage.Message == "ApuSquats" && !BotAction.IsOnMessageCooldown(chatMessage.Username, MessageType.ApuSquats))
            {
                BotAction.AddUserToMessageCooldownDictionary(chatMessage.Username, MessageType.ApuSquats);
                twitchBot.Send(chatMessage.Channel, "ApuSquats");
                BotAction.AddMessageCooldown(chatMessage.Username, MessageType.ApuSquats);
            }
        }

        public static void Ronic(TwitchBot twitchBot, ChatMessage chatMessage)
        {
            if (chatMessage.Username == "ronic76" && !BotAction.IsOnMessageCooldown(chatMessage.Username, MessageType.Ronic))
            {
                BotAction.AddUserToMessageCooldownDictionary(chatMessage.Username, MessageType.Ronic);
                twitchBot.Send(chatMessage.Channel, "SHEEEESH sheeeeeeeeeesh 🔔");
                BotAction.AddMessageCooldown(chatMessage.Username, MessageType.Ronic);
            }
        }

        public static void Bowliy(TwitchBot twitchBot, ChatMessage chatMessage)
        {
            if (chatMessage.Username == "bowliy" && !BotAction.IsOnMessageCooldown(chatMessage.Username, MessageType.Bowliy))
            {
                BotAction.AddUserToMessageCooldownDictionary(chatMessage.Username, MessageType.Bowliy);
                twitchBot.Send(chatMessage.Channel, "🤷 bowliy");
                BotAction.AddMessageCooldown(chatMessage.Username, MessageType.Bowliy);
            }
        }

        public static void Jann(TwitchBot twitchBot, ChatMessage chatMessage)
        {
            if (chatMessage.Username == "jann_amh_" && !BotAction.IsOnMessageCooldown(chatMessage.Username, MessageType.Jann))
            {
                BotAction.AddUserToMessageCooldownDictionary(chatMessage.Username, MessageType.Jann);
                twitchBot.Send(chatMessage.Channel, "👶 🍼 jann");
                BotAction.AddMessageCooldown(chatMessage.Username, MessageType.Jann);
            }
        }

        public static void HabibiTeaTime(TwitchBot twitchBot, ChatMessage chatMessage)
        {
            if (chatMessage.Message.Contains("~rando") && !BotAction.IsOnMessageCooldown(chatMessage.Username, MessageType.HabibiTeaTime))
            {
                BotAction.AddUserToMessageCooldownDictionary(chatMessage.Username, MessageType.HabibiTeaTime);
                twitchBot.Send(chatMessage.Channel, "/me STEP 1: type ~rando :tf: STEP 2: type a number over 2147483647 :tf: STEP 3: let the bot chrash :tf: ");
                BotAction.AddMessageCooldown(chatMessage.Username, MessageType.HabibiTeaTime);
            }
        }
    }
}
