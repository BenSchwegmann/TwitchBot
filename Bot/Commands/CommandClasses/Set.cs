using ApuDoingStuff.Database.Models;
using ApuDoingStuff.Twitch;
using HLE.Strings;
using System.Linq;
using TwitchLib.Client.Models;

namespace ApuDoingStuff.Commands.CommandClasses
{
    public static class Set
    {
        public static void Handle(TwitchBot twitchBot, ChatMessage chatMessage)
        {
            if (chatMessage.Message.IsMatch(@"^\?set\sbanuser\s[01](\s|$)"))
            {
                BotdbContext database = new();
                database.Banuserconfigs.FirstOrDefault(b => b.ChannelName == chatMessage.Channel).State = chatMessage.Message.Split()[2] != "0";
                database.SaveChanges();
                twitchBot.Send(chatMessage.Channel, $"/me APU banUser command has been set to {(chatMessage.Message.Split()[2] == "0" ? 0 : 1)}");
            }

            if (chatMessage.Message.IsMatch(@"^\?set\spingme\s[01](\s|$)"))
            {
                BotdbContext database = new();
                if (!database.Dicegamedbs.Any(b => b.UserName == chatMessage.Username))
                {
                    _ = database.Dicegamedbs.Add(new Dicegamedb { UserName = chatMessage.Username });
                    database.SaveChanges();

                }
                database.Dicegamedbs.FirstOrDefault(b => b.UserName == chatMessage.Username).PingMe = chatMessage.Message.Split()[2] != "0";
                database.SaveChanges();

                if (database.Dicegamedbs.FirstOrDefault(b => b.UserName == chatMessage.Username).PingMe == true)
                {
                    twitchBot.Send(chatMessage.Channel, $"/me APU you will now get a ping when your next dice is ready!");
                }
                else
                {
                    twitchBot.Send(chatMessage.Channel, $"/me APU you will not get pinged when your next dice is ready!");
                }
            }
        }
    }
}



