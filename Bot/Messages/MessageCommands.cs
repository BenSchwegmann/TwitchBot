using ApuDoingStuff.Commands;
using ApuDoingStuff.Properties;
using ApuDoingStuff.Twitch;
using HLE.Collections;
using HLE.Emojis;
using TwitchLib.Client.Models;

namespace ApuDoingStuff.Messages
{
    public static class MessageCommands
    {
        public static void ApuSquats(TwitchBot twitchBot, ChatMessage chatMessage)
        {

            if (chatMessage.Message == "ApuSquats" && !BotAction.IsOnMessageCooldown(chatMessage.Username, MessageType.ApuSquats))
            {
                BotAction.AddUserToMessageCooldownDictionary(chatMessage.Username, MessageType.ApuSquats);
                twitchBot.Send(chatMessage.Channel, "ApuSquats");
                BotAction.AddMessageCooldown(chatMessage.Username, MessageType.ApuSquats);
            }
        }

        public static void Alert(TwitchBot twitchBot, ChatMessage chatMessage)
        {
            if (chatMessage.Channel == "pajlada" && chatMessage.Username == "pajbot" && chatMessage.Message == "pajaS 🚨 ALERT" && !BotAction.IsOnMessageCooldown(chatMessage.Username, MessageType.Alert))
            {
                string[] alert = new[] { "alarm", "alarma", "аларма", "hälytys", "alarme", "συναγερμός", "aláraim", "viðvörun", "allarme", "שרעק", "signalizācija", "žadintuvas", "allarm", "аларм", "alarmă", "сигнал тревоги", "алармни", "poplach", "riasztás", "larwm", "сігнал трывогі", "տագնապ", "həyəcan", "বিপদাশঙ্কা", "နှိုးသံ", "報警", "	სიგნალიზაცია", "	એલાર્મ", "अलार्म", "アラーム", "ಎಚ್ಚರಿಕೆ", "дабыл", "ជូនដំណឹង", "경보", "ປຸກ", "	ആപല്സൂചന", "गजर", "дохиоллын", "अलार्म", "එලාම්", "	ҳушдор", "அலாரம்", "అలారం", "	สัญญาณเตือนภัย", "الارم", "signal", "báo động", "Alamu", "ƙararrawa", "alamu" };
                BotAction.AddUserToMessageCooldownDictionary(chatMessage.Username, MessageType.ApuSquats);
                twitchBot.Send(chatMessage.Channel, $"/me ApuApustaja {Emoji.RotatingLight} {alert.Random().ToUpper()}");
                twitchBot.Send(channel: Resources.pajaShh.Split()[0], Resources.pajaShh.Split()[1..].ToSequence());
                BotAction.AddMessageCooldown(chatMessage.Username, MessageType.ApuSquats);
            }
        }
    }
}
