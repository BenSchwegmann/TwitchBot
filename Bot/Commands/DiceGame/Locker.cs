using ApuDoingStuff.Database.Models;
using ApuDoingStuff.Twitch;
using HLE.Collections;
using System.Linq;
using TwitchLib.Client.Models;

namespace ApuDoingStuff.Commands.CommandClasses
{
    public static class Locker
    {
        public static void Handle(TwitchBot twitchBot, ChatMessage chatMessage)
        {
            BotdbContext database = new();
            string result = "";
            if (database.Dicegamedbs.FirstOrDefault(d => d.UserName == chatMessage.Username).Locker != null)
            {
                string[] ranks = database.Dicegamedbs.FirstOrDefault(d => d.UserName == chatMessage.Username).Locker.Split();
                ranks.ForEach(d => result += $"{d} | ");
                twitchBot.Send(chatMessage.Channel, $"/me APU @{chatMessage.Username}, you already collected these emotes: {result} [currently equipped: {database.Dicegamedbs.FirstOrDefault(d => d.UserName == chatMessage.Username).Rank} ]");
            }
            else
            {
                twitchBot.Send(chatMessage.Channel, $"/me APU @{chatMessage.Username}, you have currently no emotes in your locker. If you want to buy an emote use the \"?buy\" command");
            }
        }
    }
}
