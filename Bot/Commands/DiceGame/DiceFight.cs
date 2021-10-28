using ApuDoingStuff.Commands.DiceGame;
using ApuDoingStuff.Database.Models;
using ApuDoingStuff.Twitch;
using HLE.Strings;
using System.Linq;
using TwitchLib.Client.Models;

namespace ApuDoingStuff.Commands.CommandClasses
{
    public static class DiceFight
    {
        public static void Handle(TwitchBot twitchBot, ChatMessage chatMessage)
        {
            BotdbContext database = new();

            if (chatMessage.Message.Split().Length >= 3)
            {
                string points = chatMessage.Message.Split()[2];
                if (points.All(char.IsDigit) && points.ToInt() > 0)
                {
                    int setPoints = chatMessage.Message.Split()[2].ToInt();
                    int winnerPoints = setPoints + setPoints;
                    if (TwitchBot.FightAccepts.Any(d => d.Opponent == chatMessage.Message.Split()[1]))
                    {
                        twitchBot.Send(chatMessage.Channel, $"/me APU [ {database.Dicegamedbs.FirstOrDefault(r => r.UserName == chatMessage.Username).Rank} ] @{chatMessage.Username}, this user is already in a fight, please wait until the fight is over.");
                    }
                    else
                    {
                        if (database.Dicegamedbs.Any(d => d.UserName == chatMessage.Message.Split()[1]))
                        {
                            if (chatMessage.Message.Split()[1].ToLower() == chatMessage.Username)
                            {
                                twitchBot.Send(chatMessage.Channel, $"/me APU you can't fight yourself [ {database.Dicegamedbs.FirstOrDefault(r => r.UserName == chatMessage.Username).Rank} ] @{chatMessage.Username}");
                            }
                            else
                            {
                                string enemy = chatMessage.Message.Split()[1].ToLower();
                                FightSaveTimer saveTimer = new(chatMessage.Channel, enemy, chatMessage.Username, twitchBot);
                                if (database.Dicegamedbs.FirstOrDefault(d => d.UserName == chatMessage.Username).Points >= setPoints)
                                {
                                    if (database.Dicegamedbs.Any(d => d.UserName == enemy) && database.Dicegamedbs.FirstOrDefault(d => d.UserName == enemy).Points >= setPoints)
                                    {
                                        TwitchBot.FightAccepts.Add(new(enemy, chatMessage.Channel, setPoints, chatMessage.Username));
                                        twitchBot.Send(chatMessage.Channel, $"/me APU {$"[ {database.Dicegamedbs.FirstOrDefault(r => r.UserName == enemy).Rank} ]"} @{enemy}, you were challenged. If you want to accept please type \"?fight accept\" otherwise type \"?fight decline\"");
                                    }
                                    else
                                    {
                                        twitchBot.Send(chatMessage.Channel, $"/me APU [ {database.Dicegamedbs.FirstOrDefault(r => r.UserName == chatMessage.Username).Rank} ] @{chatMessage.Username}, your opponent has not enough points to compete [current points of @{enemy}: {database.Dicegamedbs.FirstOrDefault(d => d.UserName == enemy).Points}]");
                                    }
                                }
                                else
                                {
                                    twitchBot.Send(chatMessage.Channel, $"/me APU [ {database.Dicegamedbs.FirstOrDefault(r => r.UserName == chatMessage.Username).Rank} ] @{chatMessage.Username}, you don't have enough points to compete [current points of @{chatMessage.Username}: {database.Dicegamedbs.FirstOrDefault(d => d.UserName == chatMessage.Username).Points}]");
                                }
                            }
                        }
                        else
                        {
                            twitchBot.Send(chatMessage.Channel, $"/me APU it appears that @{chatMessage.Message.Split()[1]} has no points yet. Type \"?Dice\" to earn points.");
                        }
                    }
                }
                else
                {
                    twitchBot.Send(chatMessage.Channel, $"/me APU @{chatMessage.Username}, the set points may not be a comma number and must be positive!");
                }
            }
            else
            {
                twitchBot.Send(chatMessage.Channel, $"/me APU [ {database.Dicegamedbs.FirstOrDefault(r => r.UserName == chatMessage.Username).Rank} ] @{chatMessage.Username}, to fight against a user you need to type the username of your opponent and the points you would like to set. (eg. \"?DiceFight forsen 213\")");
            }
        }
    }
}
