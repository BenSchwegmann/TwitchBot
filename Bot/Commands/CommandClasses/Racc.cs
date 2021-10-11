using ApuDoingStuff.Database.Models;
using ApuDoingStuff.Twitch;
using System.Linq;
using TwitchLib.Client.Models;

namespace ApuDoingStuff.Commands.CommandClasses
{
    internal class Racc
    {
        public static void Handle(TwitchBot twitchBot, ChatMessage chatMessage)
        {
            BotdbContext database = new();
            twitchBot.Send(chatMessage.Channel, $"/me APU [ {database.Dicegamedbs.FirstOrDefault(r => r.UserName == chatMessage.Username).Rank} ] @{chatMessage.Username}, here you can find all kinds of information about @{chatMessage.Message.Split()[1].ToLower()}: https://emotes.raccatta.cc/twitch/{chatMessage.Message.Split()[1].ToLower()} RaccAttack");
        }
    }
}
