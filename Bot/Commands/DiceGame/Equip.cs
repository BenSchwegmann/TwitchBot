using ApuDoingStuff.Database.Models;
using ApuDoingStuff.Twitch;
using System.Linq;
using TwitchLib.Client.Models;

namespace ApuDoingStuff.Commands.CommandClasses
{
    public class Equip
    {
        public static void Handle(TwitchBot twitchBot, ChatMessage chatMessage)
        {
            BotdbContext database = new();
            if (database.Dicegamedbs.FirstOrDefault(d => d.UserName == chatMessage.Username).Locker.Split().Any(s => s == chatMessage.Message.Split()[1]))
            {
                if (chatMessage.Message.Split()[1] != database.Dicegamedbs.FirstOrDefault(d => d.UserName == chatMessage.Username).Rank)
                {
                    database.Dicegamedbs.FirstOrDefault(d => d.UserName == chatMessage.Username).Rank = chatMessage.Message.Split()[1];
                    database.SaveChanges();
                    twitchBot.Send(chatMessage.Channel, $"/me APU @{chatMessage.Username}, you succesfully equipped: {chatMessage.Message.Split()[1]}");
                }
                else
                {
                    twitchBot.Send(chatMessage.Channel, $"/me APU @{chatMessage.Username}, you currently have this emote equipped!");
                }
            }
            else
            {
                twitchBot.Send(chatMessage.Channel, $"/me APU @{chatMessage.Username}, you do not have this emote yet!");
            }

        }
    }
}
