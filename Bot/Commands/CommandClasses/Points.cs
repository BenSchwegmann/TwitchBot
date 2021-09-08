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
            twitchBot.Send(chatMessage.Channel, $"/me APU your current points are: {database.Dicegamedbs.FirstOrDefault(d => d.UserName == chatMessage.Username).Points}");
        }
    }
}
