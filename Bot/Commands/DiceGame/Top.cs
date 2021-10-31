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
            if (chatMessage.Message.Split()[1].ToInt() == 1)
            {
                Dicegamedb topPlayer = database.Dicegamedbs.OrderByDescending(d => d.Points).FirstOrDefault();
                twitchBot.Send(chatMessage.Channel, $"/me APU @{chatMessage.Username}, the best player right now is {topPlayer.UserName.Insert(2, "󠀀")} with {topPlayer.Points} points B)");
            }
            else
            {
                List<Dicegamedb> topPlayers = database.Dicegamedbs.OrderByDescending(d => d.Points).Take(chatMessage.Message.Split()[1].ToInt()).ToList();
                topPlayers.ForEach(d => result += $"{d.UserName.Insert(2, "󠀀")}, Points: {d.Points} || ");
                twitchBot.Send(chatMessage.Channel, $"/me APU @{chatMessage.Username}, the top {chatMessage.Message.Split()[1]}: {result}");
            }
        }
    }
}
