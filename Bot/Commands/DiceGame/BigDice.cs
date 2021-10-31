using ApuDoingStuff.Database.Models;
using ApuDoingStuff.Twitch;
using HLE.Emojis;
using HLE.Time;
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
            int randDice = dice.Next(-20, 36);
            if (BigDiceSaveTimer.Timers.Any(d => d.Username == chatMessage.Username))
            {
                twitchBot.Send(chatMessage.Channel, $"/me APU @{chatMessage.Username}, you can roll your next dice in {TimeHelper.ConvertUnixTimeToTimeStamp(TimeHelper.Now() - (long)BigDiceSaveTimer.Timers.FirstOrDefault(d => d.Username == chatMessage.Username).SaveTimer.RemainingTime)} || [current points of @{chatMessage.Username}: {database.Dicegamedbs.FirstOrDefault(d => d.UserName == chatMessage.Username).Points ?? 0}]");
            }
            else
            {
                BigDiceSaveTimer saveTimer = new(chatMessage.Channel, chatMessage.Username, twitchBot);
                BigDiceSaveTimer.Timers.Add(saveTimer);
                if (database.Dicegamedbs.Any(d => d.UserName == chatMessage.Username))
                {
                    database.Dicegamedbs.FirstOrDefault(d => d.UserName == chatMessage.Username).Points += randDice;
                    database.SaveChanges();
                    twitchBot.Send(chatMessage.Channel, $"{GetMessage(chatMessage.Username, randDice)} [current points of @{chatMessage.Username}: {database.Dicegamedbs.FirstOrDefault(d => d.UserName == chatMessage.Username).Points ?? 0}]");
                }
                else
                {
                    twitchBot.Send(chatMessage.Channel, $"/me APU @{chatMessage.Username}, please roll your first dice (\"?dice\") before rolling a big dice!");
                }
            }
        }

        private static string GetMessage(string username, int randDice)
        {
            if (randDice >= 30)
            {
                return $"/me APU {Emoji.ConfettiBall} WOAAAHHH APUUUU CONGRATS @{username.ToUpper()} YOU GOT AN {randDice}!!!";
            }
            else if (randDice >= 20)
            {
                return $"/me APU {Emoji.PointRight}{Emoji.PointLeft} @{username} h...here is an +{randDice} for you.";
            }
            else if (randDice >= 10)
            {
                return $"/me APU {Emoji.MagicWand} {Emoji.Sparkles} @{username} the great apu wizard gives you an well deserved {randDice}! {Emoji.Sparkles}";
            }
            else if (randDice > 0)
            {
                return $"/me FBPass APU FBBlock RUUUNNNN @{username.ToUpper()} OR YOU WILL MISS YOUR +{randDice}!";
            }
            else if (randDice <= 0)
            {
                return $"/me APU @{username} sooo unlucky you got an {randDice}! :/";
            }
            else if (randDice < -10)
            {
                return $"/me APU @{username} oh well that's sad ... you got an {randDice} :(";
            }
            else
            {
                return "/me APU";
            }
        }
    }
}
