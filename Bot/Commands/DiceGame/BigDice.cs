using ApuDoingStuff.Database.Models;
using ApuDoingStuff.Twitch;
using HLE.Emojis;
using System;
using System.Linq;
using TwitchLib.Client.Models;

namespace ApuDoingStuff.Commands.CommandClasses
{
    public static class BigDice
    {
        public static void Handle(TwitchBot twitchBot, ChatMessage chatMessage)
        {
            BotdbContext database = new();
            Random dice = new();
            int randDice = dice.Next(-21, 21);

            if (database.Dicegamedbs.Any(d => d.UserName == chatMessage.Username))
            {
                twitchBot.Send(chatMessage.Channel, GetMessage(chatMessage.Username, randDice));
                database.Dicegamedbs.FirstOrDefault(d => d.UserName == chatMessage.Username).Points += randDice;
                database.SaveChanges();
            }
            else
            {
                twitchBot.Send(chatMessage.Channel, $"/me APU @{chatMessage.Username}, please roll your first dice (\"?dice\") before rolling a big dice!");
            }
        }

        private static string GetMessage(string username, int randDice)
        {
            if (randDice == 20)
            {
                return $"/me APU {Emoji.ConfettiBall} WOAAAHHH APUUUU CONGRATS @{username} YOU GOT AN 20!!!";
            }
            else if (randDice >= 10 && randDice < 20)
            {
                return $"/me APU {Emoji.MagicWand} {Emoji.Sparkles} @{username} the great apu wizard gives you an well deserved {randDice}! {Emoji.Sparkles}";
            }
            else if (randDice > 0 && randDice < 10)
            {
                return $"/me FBPass APU FBBlock RUUUNNNN @{username.ToUpper()} OR YOU WILL MISS YOUR +{randDice}!";
            }
            else if (randDice <= 0 && randDice >= -10)
            {
                return $"/me APU @{username} sooo unlucky you got an {randDice}! :/";
            }
            else if (randDice < -10)
            {
                return $"/me APU @{username} oh well that's sad ... you got an {randDice} :(";
            }
            else
            {
                return "APU";
            }
        }
    }
}
