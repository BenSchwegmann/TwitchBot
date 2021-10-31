using ApuDoingStuff.Commands.DiceGame;
using ApuDoingStuff.Database.Models;
using ApuDoingStuff.Twitch;
using HLE.Time;
using System;
using System.Linq;
using TwitchLib.Client.Models;

namespace ApuDoingStuff.Commands.CommandClasses
{
    public static class Dice
    {
        public static void Handle(TwitchBot twitchBot, ChatMessage chatMessage)
        {
            BotdbContext database = new();
            Random dice = new();
            int randDice = dice.Next(1, 7);
            if (DiceSaveTimer.Timers.Any(d => d.Username == chatMessage.Username))
            {
                twitchBot.Send(chatMessage.Channel, $"/me APU @{chatMessage.Username}, you can roll your next dice in {TimeHelper.ConvertUnixTimeToTimeStamp(TimeHelper.Now() - (long)DiceSaveTimer.Timers.FirstOrDefault(d => d.Username == chatMessage.Username).SaveTimer.RemainingTime)} || [current points of @{chatMessage.Username}: {database.Dicegamedbs.FirstOrDefault(d => d.UserName == chatMessage.Username).Points ?? 0}]");
            }
            else
            {
                DiceSaveTimer saveTimer = new(chatMessage.Channel, chatMessage.Username, twitchBot);
                DiceSaveTimer.Timers.Add(saveTimer);
                if (database.Dicegamedbs.Any(d => d.UserName == chatMessage.Username))
                {
                    database.Dicegamedbs.FirstOrDefault(d => d.UserName == chatMessage.Username).Points += randDice;
                    database.SaveChanges();
                    twitchBot.Send(chatMessage.Channel, $"/me APU [ {database.Dicegamedbs.FirstOrDefault(r => r.UserName == chatMessage.Username).Rank} ] @{chatMessage.Username}, you got a {randDice} [current points: {database.Dicegamedbs.FirstOrDefault(d => d.UserName == chatMessage.Username).Points ?? 0}]");
                }
                else
                {
                    Console.WriteLine("hello");
                    twitchBot.Send(chatMessage.Channel, $"/me APU @{chatMessage.Username}, you got a {randDice}");
                    _ = database.Dicegamedbs.Add(new Dicegamedb { UserName = chatMessage.Username, Points = randDice, PingMe = true, Rank = "-" });
                    database.SaveChanges();
                }
            }
        }
    }
}
