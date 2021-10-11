using ApuDoingStuff.Database.Models;
using ApuDoingStuff.Properties;
using ApuDoingStuff.Twitch;
using HLE.Emojis;
using System.Linq;
using TwitchLib.Client.Models;

namespace ApuDoingStuff.Commands.CommandClasses
{
    public static class Join
    {
        public static void Handle(TwitchBot twitchBot, ChatMessage chatMessage)
        {
            if (chatMessage.Message.Split().Length >= 2)
            {
                string channel = chatMessage.Message.Split()[1];

                if (chatMessage.Username == Resources.Owner)
                {
                    BotdbContext database = new();
                    _ = database.Channels.Add(new Channel { Channel1 = $"{channel}" });
                    if (database.Channels.Any(d => d.Channel1 == channel))
                    {
                        twitchBot.Send(chatMessage.Channel, $"/me APU @{chatMessage.Username}, the bot is already connected to this channel!");
                    }
                    else
                    {
                        twitchBot.TwitchClient.JoinChannel(chatMessage.Message.Split()[1]);
                        database.SaveChanges();
                        twitchBot.Send(chatMessage.Channel, $"/me APU Bot joined channel: @{channel}");
                        twitchBot.Send(chatMessage.Message.Split()[1], $"/me APU {Emoji.ConfettiBall} bot joined!");

                    }
                }
                else
                {
                    twitchBot.Send(chatMessage.Channel, $"/me APU no, i don't think so @{chatMessage.Username} [owner only]");
                }

            }
            else
            {
                twitchBot.Send(chatMessage.Channel, $"/me APU @{chatMessage.Username}, this is an invalid username");
            }
        }
    }
}
