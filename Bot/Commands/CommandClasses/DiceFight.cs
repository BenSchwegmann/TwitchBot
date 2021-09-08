using ApuDoingStuff.Commands.DiceGame;
using ApuDoingStuff.Database.Models;
using ApuDoingStuff.Twitch;
using HLE.Collections;
using System;
using System.Linq;
using TwitchLib.Client.Models;
using HLE.Emojis;

namespace ApuDoingStuff.Commands.CommandClasses
{
    public static class DiceFight
    {
        public static void Handle(TwitchBot twitchBot, ChatMessage chatMessage)
        {
            BotdbContext database = new();
            int setPoints = Convert.ToInt32(chatMessage.Message.Split()[2]);
            int winnerPoints = setPoints + setPoints;
            string enemy = chatMessage.Message.Split()[1].ToLower();

            if (database.Dicegamedbs.FirstOrDefault(d => d.UserName == chatMessage.Username).Points >= setPoints)
            {
                if (database.Dicegamedbs.Any(d => d.UserName == enemy) && database.Dicegamedbs.FirstOrDefault(d => d.UserName == enemy).Points >= setPoints)
                {
                    twitchBot.Send(chatMessage.Channel, $"/me APU @{enemy}, you were challenged. If you want to accept please type \"?fight accept\" otherwise type \"?fight decline\"");
                    FightAccept aFight = new(true);
                    if (aFight.Accepted)
                    {
                        string[] opponents = { enemy, chatMessage.Username };
                        string winner = opponents.Random();
                        Console.WriteLine(winner);
                        database.Dicegamedbs.FirstOrDefault(d => d.UserName == chatMessage.Username).Points -= setPoints;
                        database.Dicegamedbs.FirstOrDefault(d => d.UserName == enemy).Points -= setPoints;
                        database.Dicegamedbs.FirstOrDefault(d => d.UserName == winner).Points += winnerPoints;
                        database.SaveChanges();
                        twitchBot.Send(chatMessage.Channel, $"/me APU {Emoji.ConfettiBall} congratulations @{winner} you just won {setPoints + setPoints} points!");
                        if (winner == enemy)
                        {
                            if (setPoints == 1)
                            {
                                twitchBot.Send(chatMessage.Channel, $"/me APU unlucky i guess @{chatMessage.Username}. You just lost {setPoints} point :(");
                            }
                            else
                            {
                                twitchBot.Send(chatMessage.Channel, $"/me APU unlucky i guess @{chatMessage.Username}. You just lost {setPoints} points :(");
                            }
                        }
                        else if (winner == chatMessage.Username)
                        {
                            if (setPoints == 1)
                            {
                                twitchBot.Send(chatMessage.Channel, $"/me APU unlucky i guess @{enemy}. You just lost {setPoints} point :(");
                            }
                            else
                            {
                                twitchBot.Send(chatMessage.Channel, $"/me APU unlucky i guess @{enemy}. You just lost {setPoints} points :(");
                            }
                        }
                    }
                }
                else
                {
                    twitchBot.Send(chatMessage.Channel, $"/me APU @{chatMessage.Username}, your opponent has not enough points to compete [current points of @{enemy}: {database.Dicegamedbs.FirstOrDefault(d => d.UserName == enemy).Points}]");
                }
            }
            else
            {
                twitchBot.Send(chatMessage.Channel, $"/me APU @{chatMessage.Username}, you don't have enough points to compete [current points of @{chatMessage.Username}: {database.Dicegamedbs.FirstOrDefault(d => d.UserName == chatMessage.Username).Points}]");
            }


        }
    }
}
