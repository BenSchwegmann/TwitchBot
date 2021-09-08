using ApuDoingStuff.Twitch;
using TwitchLib.Client.Models;

namespace ApuDoingStuff.Commands.CommandClasses
{
    public static class Bryan
    {
        public static void Handle(TwitchBot twitchBot, ChatMessage chatMessage)
        {
            if (chatMessage.Channel == "benASTRO")
            {
                twitchBot.Send(chatMessage.Channel, $"/me APU @{chatMessage.Username}, https://i.imgur.com/yxXqMSP.png");
            }
        }
    }
}
