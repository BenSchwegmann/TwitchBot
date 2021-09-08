using ApuDoingStuff.Commands.DiceGame;
using ApuDoingStuff.Twitch;
using HLE.Strings;
using TwitchLib.Client.Models;

namespace ApuDoingStuff.Commands.CommandClasses
{
    public static class Fight
    {

        public static void Handle(TwitchBot twitchBot, ChatMessage chatMessage)
        {
            FightAccept aFight = new(true);
            if (chatMessage.Message.IsMatch("?fight accept"))
            {
                aFight.Accepted = true;
            }

            if (chatMessage.Message.IsMatch("?fight decline"))
            {
                aFight.Accepted = false;
            }
        }
    }
}

