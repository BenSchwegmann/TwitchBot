using ApuDoingStuff.API;
using ApuDoingStuff.Twitch;
using HLE.Collections;
using TwitchLib.Client.Models;

namespace ApuDoingStuff.Commands.CommandClasses
{
    public class Lyrics
    {
        public static void Handle(TwitchBot twitchBot, ChatMessage chatMessage)
        {
            string title = chatMessage.Message.Split()[1..].ToSequence();
            string result = HTTPRequest.LyricsUrl(title);
            twitchBot.Send(chatMessage.Channel, $"/me APU @{chatMessage.Username}, {result}");
        }
    }
}
