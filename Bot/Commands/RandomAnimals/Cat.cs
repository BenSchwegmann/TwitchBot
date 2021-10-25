using ApuDoingStuff.API;
using ApuDoingStuff.Twitch;
using TwitchLib.Client.Models;

namespace ApuDoingStuff.Commands.CommandClasses
{
    public class Cat
    {
        public static void Handle(TwitchBot twitchBot, ChatMessage chatMessage)
        {
            twitchBot.Send(chatMessage.Channel, $"/me APU @{chatMessage.Username}, {HTTPRequest.RandomCatFact()} {HTTPRequest.RandomCatUrl()} CoolCat");
        }
    }
}
