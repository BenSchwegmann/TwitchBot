using ApuDoingStuff.Database.Models;
using ApuDoingStuff.Properties;
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
            BotdbContext database = new();
            if (chatMessage.Message.IsMatch(@"^\?set\spingme\s[01](\s|$)"))
            {
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

            if (chatMessage.Message.IsMatch(@"^\?set\slive\s[01](\s|$)"))
            {
                if (chatMessage.IsModerator || chatMessage.IsBroadcaster || chatMessage.Username == Resources.Owner)
                {
                    database.Channels.FirstOrDefault(d => d.Channel1 == chatMessage.Channel).IfLive = chatMessage.Message.Split()[2] != "0";
                    database.SaveChanges();
                    if (chatMessage.Message.Split()[2] == "0")
                    {
                        twitchBot.Send(chatMessage.Channel, $"/me APU @{chatMessage.Username}, you can use the bot while the channel is live!");
                    }
                    else
                    {
                        twitchBot.Send(chatMessage.Channel, $"/me APU @{chatMessage.Username}, you can not use the bot while the channel is live!");
                    }
                }
                else
                {
                    twitchBot.Send(chatMessage.Channel, $"/me APU @{chatMessage.Username}, this command is for moderators/broadcaster only!");
                }
            }
        }
    }
}



