using ApuDoingStuff.Twitch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchLib.Client.Models;

namespace ApuDoingStuff.Commands.CommandClasses
{
    class Commands
    {
        public static void Handle (TwitchBot twitchBot, ChatMessage chatMessage)
        {
            twitchBot.Send(chatMessage.Channel, $"/me APU @{chatMessage.Username}, here you can find all commands: https://github.com/benASTRO/ApuDoingStuff");
        }
    }
}
