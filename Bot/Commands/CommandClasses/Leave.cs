using ApuDoingStuff.Database.Models;
using ApuDoingStuff.Properties;
using ApuDoingStuff.Twitch;
using System.Linq;
using TwitchLib.Client.Models;
using HLE.Emojis;

namespace ApuDoingStuff.Commands.CommandClasses
{
    public static class Leave
    {
        public static void Handle(TwitchBot twitchBot, ChatMessage chatMessage)
        {
            if (chatMessage.IsModerator || chatMessage.IsBroadcaster)
            {
                twitchBot.TwitchClient.LeaveChannel(chatMessage.Channel);
                BotdbContext database = new();
                database.Channels.Remove(database.Channels.FirstOrDefault(d => d.Channel1 == chatMessage.Channel));
                database.SaveChanges();
                twitchBot.Send(chatMessage.Channel, $"/me APU {Emoji.Wave} bye bye");
            }
            else
            {
                twitchBot.Send(chatMessage.Channel, $"/me APU no, i don't think so @{chatMessage.Username} [moderator/broadcaster only]");
            }

            if (chatMessage.Username == Resources.Owner)
            {
                twitchBot.TwitchClient.LeaveChannel(chatMessage.Message.Split()[1]);
                BotdbContext database = new();
                database.Channels.Remove(database.Channels.FirstOrDefault(d => d.Channel1 == chatMessage.Message.Split()[1]));
                database.SaveChanges();
                twitchBot.Send(channel: "apudoingstuff", $"/me APU the bot left channel: @{chatMessage.Message.Split()[1]}");
            }
        }
    }
}
