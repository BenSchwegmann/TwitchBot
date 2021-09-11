using ApuDoingStuff.Database.Models;
using ApuDoingStuff.Twitch;
using HLE.Collections;
using HLE.Emojis;
using System;
using System.Linq;
using TwitchLib.Client.Models;

namespace ApuDoingStuff.Commands.CommandClasses
{
    public static class DiceFight
    {
        public static void Handle(TwitchBot twitchBot, ChatMessage chatMessage)
        {
            BotdbContext database = new();
            Console.WriteLine(chatMessage.Message.Split()[1]);

            if (chatMessage.Message.Split().Length >= 3)
            {
                if (chatMessage.Message.Split()[1].ToLower() == chatMessage.Username)
                {
                    twitchBot.Send(chatMessage.Channel, $"/me APU you can't fight yourself @{chatMessage.Username}");
                }
                else
                {
                    int setPoints = Convert.ToInt32(chatMessage.Message.Split()[2]);
                    int winnerPoints = setPoints + setPoints;
                    string enemy = chatMessage.Message.Split()[1].ToLower();
                    if (database.Dicegamedbs.FirstOrDefault(d => d.UserName == chatMessage.Username).Points >= setPoints)
                    {
                        if (database.Dicegamedbs.Any(d => d.UserName == enemy) && database.Dicegamedbs.FirstOrDefault(d => d.UserName == enemy).Points >= setPoints)
                        {
                            twitchBot.Send(chatMessage.Channel, $"/me APU @{enemy}, you were challenged. If you want to accept please type \"?fight accept\" otherwise type \"?fight decline\"");
                            TwitchBot.FightAccepts.Add(new(enemy, chatMessage.Channel, setPoints, chatMessage.Username));
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
            else
            {
                twitchBot.Send(chatMessage.Channel, $"/me APU @{chatMessage.Username}, to fight against a user you need to type the username of your opponent and your point you would like to set. (eg. \"?DiceFight forsen 213\")");
            }
        }
    }
}
