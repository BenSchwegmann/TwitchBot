using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApuDoingStuff.Twitch;
using TwitchLib.Client.Models;
using ApuDoingStuff.Commands;

namespace ApuDoingStuff.Messages
{
    public static class MessageCommands
    {

        public static void Laurin(TwitchBot twitchBot, ChatMessage chatMessage)
        {
            if (chatMessage.Username == "lauriin")
            {
                BotAction.AddUserToMessageCooldownDictionary(chatMessage.Username, MessageType.Laurin);
                twitchBot.Send(chatMessage.Channel, "Laurin hat nen kleinen Schniedel AlienPls");
                BotAction.AddMessageCooldown(chatMessage.Username, MessageType.Laurin);

            }
        }

        public static void ApuSquats(TwitchBot twitchBot, ChatMessage chatMessage)
        {
            if (chatMessage.Message == "ApuSquats")
            {
                twitchBot.Send(chatMessage.Channel, "ApuSquats");
            }
        }

        public static void Ronic(TwitchBot twitchBot, ChatMessage chatMessage)
        {
            if (chatMessage.Username == "ronic76")
            {
                twitchBot.Send(chatMessage.Channel, "SHEEEESH sheeeeeeeeeesh 🔔 ");
            }
        }

        public static void Bowliy(TwitchBot twitchBot, ChatMessage chatMessage)
        {
            if (chatMessage.Username == "bowliy")
            {
                twitchBot.Send(chatMessage.Channel, "bowliy Bowliy BOWLIY ");
            }
        }
    }
}
