using ApuDoingStuff.Database.Models;
using ApuDoingStuff.Twitch;
using HLE.Collections;
using HLE.Emojis;
using HLE.Strings;
using System.Linq;
using TwitchLib.Client.Models;

namespace ApuDoingStuff.Commands.CommandClasses
{
    public static class Fight
    {

        public static void Handle(TwitchBot twitchBot, ChatMessage chatMessage)
        {
            if (TwitchBot.FightAccepts.Any(d => d.Opponent == chatMessage.Username))
            {
                string challenger = TwitchBot.FightAccepts.FirstOrDefault(d => d.Opponent == chatMessage.Username).Challenger;
                int points = TwitchBot.FightAccepts.FirstOrDefault(d => d.Opponent == chatMessage.Username).Points;
                int winnerPoints = points + points;

                if (chatMessage.Message.IsMatch("^?fight accept") && TwitchBot.FightAccepts.Any(d => d.Opponent == chatMessage.Username))
                {
                    TwitchBot.FightAccepts.FirstOrDefault(d => d.Opponent == chatMessage.Username).Accepted = true;
                    BotdbContext database = new();

                    string[] opponents = { challenger, chatMessage.Username };
                    string winner = opponents.Random();
                    database.Dicegamedbs.FirstOrDefault(d => d.UserName == challenger).Points -= points;
                    database.Dicegamedbs.FirstOrDefault(d => d.UserName == chatMessage.Username).Points -= points;
                    database.Dicegamedbs.FirstOrDefault(d => d.UserName == winner).Points += winnerPoints;
                    database.SaveChanges();
                    twitchBot.Send(chatMessage.Channel, $"/me APU {Emoji.ConfettiBall} congratulations @{winner} you just won {winnerPoints} points!");
                    System.Console.WriteLine(challenger);
                    if (winner == challenger)
                    {
                        if (points == 1)
                        {
                            twitchBot.Send(chatMessage.Channel, $"/me APU unlucky i guess @{chatMessage.Username}. You lost {points} point :(");
                        }
                        else
                        {
                            twitchBot.Send(chatMessage.Channel, $"/me APU unlucky i guess @{chatMessage.Username}. You lost {points} points :(");
                        }
                    }
                    else if (winner == chatMessage.Username)
                    {
                        if (points == 1)
                        {
                            twitchBot.Send(chatMessage.Channel, $"/me APU unlucky i guess @{challenger}. You lost {points} point :(");
                        }
                        else
                        {
                            twitchBot.Send(chatMessage.Channel, $"/me APU unlucky i guess @{challenger}. You lost {points} points :(");
                        }
                    }
                    TwitchBot.FightAccepts.Remove(TwitchBot.FightAccepts.FirstOrDefault(d => d.Opponent == chatMessage.Username));
                }

                if (chatMessage.Message.IsMatch("^?fight decline"))
                {
                    TwitchBot.FightAccepts.FirstOrDefault(d => d.Accepted = false);
                    TwitchBot.FightAccepts.Remove(TwitchBot.FightAccepts.FirstOrDefault(d => d.Opponent == chatMessage.Username));
                    twitchBot.Send(chatMessage.Channel, $"/me APU @{challenger}, your opponent denied the fight.");
                }
            }
            else
            {
                BotdbContext database = new();
                twitchBot.Send(chatMessage.Channel, $"/me APU {$"[ {database.Dicegamedbs.FirstOrDefault(r => r.UserName == chatMessage.Username).Rank} ]"} @{chatMessage.Username}, you have not been challenged.");
            }
        }
    }
}

