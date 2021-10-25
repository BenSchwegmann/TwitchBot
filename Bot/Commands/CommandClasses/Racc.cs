using ApuDoingStuff.Twitch;
using TwitchLib.Client.Models;

namespace ApuDoingStuff.Commands.CommandClasses
{
    internal class Racc
    {
        public static void Handle(TwitchBot twitchBot, ChatMessage chatMessage)
        {
            twitchBot.Send(chatMessage.Channel, $"/me APU @{chatMessage.Username}, here you can find all kinds of information about @{chatMessage.Message.Split()[1].ToLower()}: https://emotes.raccatta.cc/twitch/{chatMessage.Message.Split()[1].ToLower()} RaccAttack");
        }
    }
}
