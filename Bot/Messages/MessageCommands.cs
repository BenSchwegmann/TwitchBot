using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApuDoingStuff.Twitch;
using TwitchLib.Client.Models;

namespace ApuDoingStuff.Messages
{
    public static class MessageCommands
    {
        public static void Laurin(TwitchBot twitchBot, ChatMessage chatMessage)
        {
            if (chatMessage.Username == "lauriin")
            {
                twitchBot.Send(chatMessage.Channel, "Laurin hat nen kleinen Schniedel AlienPls");
            }
        }
    }
}
