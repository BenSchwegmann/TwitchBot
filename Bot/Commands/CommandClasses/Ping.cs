using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchLib.Client.Models;
using ApuDoingStuff.Commands;
using ApuDoingStuff.Twitch;

namespace ApuDoingStuff.Commands.CommandClasses
{
    public static class Ping
    {
        public static void Handle(TwitchBot twitchBot,ChatMessage chatMessage)
        {
            twitchBot.Send(chatMessage.Channel, $"ApuSpin PONG! {twitchBot.GetSystemInfo()}");

        }
            
    }
}
