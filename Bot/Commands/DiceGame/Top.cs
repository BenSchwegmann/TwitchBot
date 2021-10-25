using ApuDoingStuff.Database.Models;
using ApuDoingStuff.Twitch;
using HLE.Strings;
using System.Collections.Generic;
using System.Linq;
using TwitchLib.Client.Models;

namespace ApuDoingStuff.Commands.CommandClasses
{
    public static class Top
    {
        public static void Handle(TwitchBot twitchBot, ChatMessage chatMessage)
        {
            BotdbContext database = new();
            string result = "";
            List<Dicegamedb> topPlayers = database.Dicegamedbs.OrderByDescending(d => d.Points).Take(chatMessage.Message.Split()[1].ToInt()).ToList();
            topPlayers.ForEach(d => result += $"{d.UserName}, Points: {d.Points} || ");
            twitchBot.Send(chatMessage.Channel, $"/me APU @{chatMessage.Username}, the top {chatMessage.Message.Split()[1]}: {result}");
        }
    }
}
