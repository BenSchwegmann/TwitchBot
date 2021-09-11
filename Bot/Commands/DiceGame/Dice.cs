using ApuDoingStuff.Commands.DiceGame;
using ApuDoingStuff.Database.Models;
using ApuDoingStuff.Twitch;
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


            if (database.Dicegamedbs.Any(d => d.UserName == chatMessage.Username))
            {
                Console.WriteLine(chatMessage.Username);
                database.Dicegamedbs.FirstOrDefault(d => d.UserName == chatMessage.Username).Points += randDice;
                database.SaveChanges();
                twitchBot.Send(chatMessage.Channel, $"/me APU @{chatMessage.Username}, you got a {randDice} [current points: {database.Dicegamedbs.FirstOrDefault(d => d.UserName == chatMessage.Username).Points ?? 0}]");
                DiceSaveTimer saveTimer = new(chatMessage.Channel, chatMessage.Username, twitchBot);

            }
            else
            {
                twitchBot.Send(chatMessage.Channel, $"/me APU @{chatMessage.Username}, you got a {randDice}");
                _ = database.Dicegamedbs.Add(new Dicegamedb { UserName = chatMessage.Username, Points = randDice });
                database.SaveChanges();
                database.Dicegamedbs.FirstOrDefault(d => d.UserName == chatMessage.Username).PingMe = true;
                database.SaveChanges();
                DiceSaveTimer saveTimer = new(chatMessage.Channel, chatMessage.Username, twitchBot);
            }
        }
    }
}
