using ApuDoingStuff.Twitch;
using HLE.Emojis;
using TwitchLib.Client.Models;

namespace ApuDoingStuff.Commands.CommandClasses
{
    public static class Ping
    {
        public static void Handle(TwitchBot twitchBot, ChatMessage chatMessage)
        {
            twitchBot.Send(chatMessage.Channel, $"/me APU {Emoji.MagicWand} PONG! {twitchBot.GetRuntime()}");

        }

    }
}
