using ApuDoingStuff.Database.Models;
using ApuDoingStuff.Twitch;
using System.Linq;
using TwitchLib.Client.Models;

namespace ApuDoingStuff.Commands.CommandClasses
{
    public class Points
    {
        public static void Handle(TwitchBot twitchBot, ChatMessage chatMessage)
        {
            BotdbContext database = new();
            if (chatMessage.Message.Split().Length == 1)
            {
                twitchBot.Send(chatMessage.Channel, $"/me APU {$"[ {database.Dicegamedbs.FirstOrDefault(r => r.UserName == chatMessage.Username).Rank} ]"} @{chatMessage.Username} your current points: {database.Dicegamedbs.FirstOrDefault(d => d.UserName == chatMessage.Username).Points}");
            }
            else
            {
                twitchBot.Send(chatMessage.Channel, $"/me APU {$"[ {database.Dicegamedbs.FirstOrDefault(r => r.UserName == chatMessage.Message.Split()[1]).Rank} ]"} @{chatMessage.Message.Split()[1]} current points: {database.Dicegamedbs.FirstOrDefault(d => d.UserName == chatMessage.Message.Split()[1]).Points}");
            }
        }
    }
}
