using ApuDoingStuff.Commands.CommandClasses;
using ApuDoingStuff.Commands.DiceGame;
using ApuDoingStuff.Database.Models;
using ApuDoingStuff.Jsons;
using HLE.Collections;
using HLE.Emojis;
using HLE.Strings;
using HLE.Time;
using System;
using System.Collections.Generic;
using System.Linq;
using TwitchLib.Client.Models;

namespace ApuDoingStuff.Twitch
{
    public class DiceAction
    {
        private static readonly BotdbContext _database = new();
        public static string GetBigDice(ChatMessage chatMessage, TwitchBot twitchBot)
        {
            BotdbContext database = new();
            Random dice = new();
            int randDice = dice.Next(-20, 36);
            if (BigDiceSaveTimer.Timers.Any(d => d.Username == chatMessage.Username))
            {
                return $"/me APU @{chatMessage.Username}, you can roll your next dice in {TimeHelper.ConvertUnixTimeToTimeStamp(TimeHelper.Now() - (long)BigDiceSaveTimer.Timers.FirstOrDefault(d => d.Username == chatMessage.Username).SaveTimer.RemainingTime)} || [current points of @{chatMessage.Username}: {database.Dicegamedbs.FirstOrDefault(d => d.UserName == chatMessage.Username).Points ?? 0}]";
            }
            else
            {
                BigDiceSaveTimer saveTimer = new(chatMessage.Channel, chatMessage.Username, twitchBot);
                BigDiceSaveTimer.Timers.Add(saveTimer);
                if (database.Dicegamedbs.Any(d => d.UserName == chatMessage.Username))
                {
                    // database.Dicegamedbs.FirstOrDefault(d => d.UserName == chatMessage.Username).Points += randDice;
                    database.SaveChanges();
                    return $"{GetMessage(chatMessage.Username, randDice)} [current points of @{chatMessage.Username}: {database.Dicegamedbs.FirstOrDefault(d => d.UserName == chatMessage.Username).Points ?? 0}]";
                }
                else
                {
                    return $"/me APU @{chatMessage.Username}, please roll your first dice (\"?dice\") before rolling a big dice!";
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
        public static string SendBuy(ChatMessage chatMessage)
        {
            if (chatMessage.Message.Split().Length >= 2)
            {
                int emoteNr = chatMessage.Message.Split()[1].ToInt();
                if (chatMessage.Message.Split()[1].IsMatch(@"^\d+$") && JsonController.Ranks.Any(r => r.EmoteNr == emoteNr))
                {
                    foreach (Rank r in JsonController.Ranks)
                    {
                        if (r.EmoteNr == emoteNr)
                        {
                            if (!_database.Dicegamedbs.FirstOrDefault(d => d.UserName == chatMessage.Username).EmoteNr.Split().Any(s => s == chatMessage.Message.Split()[1]))
                            {
                                if (_database.Dicegamedbs.FirstOrDefault(d => d.UserName == chatMessage.Username).Points >= r.Price)
                                {
                                    _ = _database.Dicegamedbs.FirstOrDefault(d => d.UserName == chatMessage.Username).Rank = r.Name;
                                    _database.Dicegamedbs.FirstOrDefault(d => d.UserName == chatMessage.Username).Points -= r.Price;
                                    _database.Dicegamedbs.FirstOrDefault(d => d.UserName == chatMessage.Username).EmoteNr += $"{r.EmoteNr} ";
                                    _database.Dicegamedbs.FirstOrDefault(d => d.UserName == chatMessage.Username).Locker += $" {r.Name}";
                                    _database.SaveChanges();
                                    return $"/me APU {Emoji.Tada} congratulations @{chatMessage.Username} you bought the emote: {r.Name}";
                                }
                                else
                                {
                                    return $"/me APU @{chatMessage.Username}, you don't have enough points to buy this emote!";
                                }
                            }
                            else
                            {
                                return $"/me APU @{chatMessage.Username}, you already own this emote! (\"?locker\" to get a list of all your emotes";
                            }
                        }
                        else
                        {
                            return string.Empty;
                        }
                    }
                    return string.Empty;
                }
                else
                {
                    return $"/me APU @{chatMessage.Username}, this is not a valid emote number! Type \"?shop\" to see a list of all emotes, emote numbers and their prices.";
                }
            }
            else
            {
                return $"/me APU @{chatMessage.Username}, please enter the emote number! Type \"?shop\" to see a list of all emotes, emote numbers and their prices.";
            }
        }

        public static string SendDice(ChatMessage chatMessage, TwitchBot twitchBot)
        {
            Random dice = new();
            int randDice = dice.Next(1, 7);
            if (DiceSaveTimer.Timers.Any(d => d.Username == chatMessage.Username))
            {
                return $"/me APU @{chatMessage.Username}, you can roll your next dice in {TimeHelper.ConvertUnixTimeToTimeStamp(TimeHelper.Now() - (long)DiceSaveTimer.Timers.FirstOrDefault(d => d.Username == chatMessage.Username).SaveTimer.RemainingTime)} || [current points of @{chatMessage.Username}: {_database.Dicegamedbs.FirstOrDefault(d => d.UserName == chatMessage.Username).Points ?? 0}]";
            }
            else
            {
                DiceSaveTimer saveTimer = new(chatMessage.Channel, chatMessage.Username, twitchBot);
                DiceSaveTimer.Timers.Add(saveTimer);
                if (_database.Dicegamedbs.Any(d => d.UserName == chatMessage.Username))
                {
                    // database.Dicegamedbs.FirstOrDefault(d => d.UserName == chatMessage.Username).Points += randDice;
                    _database.SaveChanges();
                    return $"/me APU [ {_database.Dicegamedbs.FirstOrDefault(r => r.UserName == chatMessage.Username).Rank} ] @{chatMessage.Username}, you got a {randDice} [current points: {_database.Dicegamedbs.FirstOrDefault(d => d.UserName == chatMessage.Username).Points ?? 0}]";
                }
                else
                {
                    _ = _database.Dicegamedbs.Add(new Dicegamedb { UserName = chatMessage.Username, Points = randDice, PingMe = true, Rank = "-" });
                    _database.SaveChanges();
                    return $"/me APU @{chatMessage.Username}, you got a {randDice}";
                }
            }
        }

        public static string SendDiceFight(ChatMessage chatMessage, TwitchBot twitchBot)
        {
            if (chatMessage.Message.Split().Length >= 3)
            {
                string points = chatMessage.Message.Split()[2];
                if (points.All(char.IsDigit) && points.ToInt() > 0)
                {
                    int setPoints = chatMessage.Message.Split()[2].ToInt();
                    int winnerPoints = setPoints + setPoints;
                    if (TwitchBot.FightAccepts.Any(d => d.Opponent == chatMessage.Message.Split()[1]))
                    {
                        return $"/me APU [ {_database.Dicegamedbs.FirstOrDefault(r => r.UserName == chatMessage.Username).Rank} ] @{chatMessage.Username}, this user is already in a fight, please wait until the fight is over.";
                    }
                    else
                    {
                        if (_database.Dicegamedbs.Any(d => d.UserName == chatMessage.Message.Split()[1]))
                        {
                            if (chatMessage.Message.Split()[1].ToLower() == chatMessage.Username)
                            {
                                return $"/me APU you can't fight yourself [ {_database.Dicegamedbs.FirstOrDefault(r => r.UserName == chatMessage.Username).Rank} ] @{chatMessage.Username}";
                            }
                            else
                            {
                                string enemy = chatMessage.Message.Split()[1].ToLower();
                                if (_database.Dicegamedbs.FirstOrDefault(d => d.UserName == chatMessage.Username).Points >= setPoints)
                                {
                                    if (_database.Dicegamedbs.Any(d => d.UserName == enemy) && _database.Dicegamedbs.FirstOrDefault(d => d.UserName == enemy).Points >= setPoints)
                                    {
                                        TwitchBot.FightAccepts.Add(new(enemy, chatMessage.Channel, setPoints, chatMessage.Username));
                                        FightSaveTimer saveTimer = new(chatMessage.Channel, enemy, chatMessage.Username, twitchBot);
                                        return $"/me APU {$"[ {_database.Dicegamedbs.FirstOrDefault(r => r.UserName == enemy).Rank} ]"} @{enemy}, you were challenged. If you want to accept please type \"?fight accept\" otherwise type \"?fight decline\"";
                                    }
                                    else
                                    {
                                        return $"/me APU [ {_database.Dicegamedbs.FirstOrDefault(r => r.UserName == chatMessage.Username).Rank} ] @{chatMessage.Username}, your opponent has not enough points to compete [current points of @{enemy}: {_database.Dicegamedbs.FirstOrDefault(d => d.UserName == enemy).Points}]";
                                    }
                                }
                                else
                                {
                                    return $"/me APU [ {_database.Dicegamedbs.FirstOrDefault(r => r.UserName == chatMessage.Username).Rank} ] @{chatMessage.Username}, you don't have enough points to compete [current points of @{chatMessage.Username}: {_database.Dicegamedbs.FirstOrDefault(d => d.UserName == chatMessage.Username).Points}]";
                                }
                            }
                        }
                        else
                        {
                            return $"/me APU it appears that @{chatMessage.Message.Split()[1]} has no points yet. Type \"?Dice\" to earn points.";
                        }
                    }
                }
                else
                {
                    return $"/me APU @{chatMessage.Username}, the set points may not be a comma number and must be positive!";
                }
            }
            else
            {
                return $"/me APU [ {_database.Dicegamedbs.FirstOrDefault(r => r.UserName == chatMessage.Username).Rank} ] @{chatMessage.Username}, to fight against a user you need to type the username of your opponent and the points you would like to set. (eg. \"?DiceFight forsen 213\")";
            }
        }

        public static string SendEquip(ChatMessage chatMessage)
        {
            if (_database.Dicegamedbs.FirstOrDefault(d => d.UserName == chatMessage.Username).Locker.Split().Any(s => s == chatMessage.Message.Split()[1]))
            {
                if (chatMessage.Message.Split()[1] != _database.Dicegamedbs.FirstOrDefault(d => d.UserName == chatMessage.Username).Rank)
                {
                    _database.Dicegamedbs.FirstOrDefault(d => d.UserName == chatMessage.Username).Rank = chatMessage.Message.Split()[1];
                    _database.SaveChanges();
                    return $"/me APU @{chatMessage.Username}, you succesfully equipped: {chatMessage.Message.Split()[1]}";
                }
                else
                {
                    return $"/me APU @{chatMessage.Username}, you currently have this emote equipped!";
                }
            }
            else
            {
                return $"/me APU @{chatMessage.Username}, you do not have this emote yet!";
            }
        }

        public static string SendFight(ChatMessage chatMessage)
        {
            if (TwitchBot.FightAccepts.Any(d => d.Opponent == chatMessage.Username))
            {
                string challenger = TwitchBot.FightAccepts.FirstOrDefault(d => d.Opponent == chatMessage.Username).Challenger;
                int points = TwitchBot.FightAccepts.FirstOrDefault(d => d.Opponent == chatMessage.Username).Points;
                int winnerPoints = points + points;

                if (chatMessage.Message.IsMatch("^?fight accept") && TwitchBot.FightAccepts.Any(d => d.Opponent == chatMessage.Username))
                {
                    TwitchBot.FightAccepts.FirstOrDefault(d => d.Opponent == chatMessage.Username).Accepted = true;

                    string[] opponents = { challenger, chatMessage.Username };
                    string winner = opponents.Random();
                    _database.Dicegamedbs.FirstOrDefault(d => d.UserName == challenger).Points -= points;
                    _database.Dicegamedbs.FirstOrDefault(d => d.UserName == chatMessage.Username).Points -= points;
                    _database.Dicegamedbs.FirstOrDefault(d => d.UserName == winner).Points += winnerPoints;
                    _database.SaveChanges();
                    if (winner == challenger)
                    {
                        if (points == 1)
                        {
                            return $"/me APU unlucky i guess @{chatMessage.Username}. You lost {points} point :(";
                        }
                        else
                        {
                            return $"/me APU unlucky i guess @{chatMessage.Username}. You lost {points} points :(";
                        }
                    }
                    else if (winner == chatMessage.Username)
                    {
                        if (points == 1)
                        {
                            return $"/me APU unlucky i guess @{challenger}. You lost {points} point :(";
                        }
                        else
                        {
                            return $"/me APU unlucky i guess @{challenger}. You lost {points} points :(";
                        }
                    }
                    TwitchBot.FightAccepts.Remove(TwitchBot.FightAccepts.FirstOrDefault(d => d.Opponent == chatMessage.Username));
                    return $"/me APU {Emoji.ConfettiBall} congratulations @{winner} you just won {winnerPoints} points!";
                }

                if (chatMessage.Message.IsMatch("^?fight decline"))
                {
                    TwitchBot.FightAccepts.FirstOrDefault(d => d.Accepted = false);
                    TwitchBot.FightAccepts.Remove(TwitchBot.FightAccepts.FirstOrDefault(d => d.Opponent == chatMessage.Username));
                    return $"/me APU @{challenger}, your opponent denied the fight.";
                }
                else
                {
                    return string.Empty;
                }
            }
            else
            {
                BotdbContext database = new();
                return $"/me APU {$"[ {database.Dicegamedbs.FirstOrDefault(r => r.UserName == chatMessage.Username).Rank} ]"} @{chatMessage.Username}, you have not been challenged.";
            }
        }

        public static string SendGift(ChatMessage chatMessage)
        {
            if (chatMessage.Message.Split().Length == 3)
            {
                if (chatMessage.Message.Split()[1].ToLower() == chatMessage.Username)
                {
                    return $"/me APU @{chatMessage.Username}, you can't gift yourself points!";
                }
                else
                {

                    if (chatMessage.Message.Split()[2].All(char.IsDigit))
                    {
                        int points = chatMessage.Message.Split()[2].ToInt();
                        if (points <= 10)
                        {
                            if (points == 1)
                            {
                                if (_database.Dicegamedbs.Any(d => d.UserName == chatMessage.Message.Split()[1]) && _database.Dicegamedbs.FirstOrDefault(d => d.UserName == chatMessage.Username).Points >= 0)
                                {
                                    _database.Dicegamedbs.FirstOrDefault(d => d.UserName == chatMessage.Username).Points -= points;
                                    _database.Dicegamedbs.FirstOrDefault(d => d.UserName == chatMessage.Message.Split()[1]).Points += points;
                                    _database.SaveChanges();
                                    return $"/me APU @{chatMessage.Username} you gifted @{chatMessage.Message.Split()[1]} {points} points!";
                                }
                                else
                                {
                                    return $"/me APU it appears that @{chatMessage.Username} has no points yet. Type \"?Dice\" to earn points.";
                                }
                            }
                            else
                            {
                                return string.Empty;
                            }
                        }
                        else
                        {
                            return $"/me APU @{chatMessage.Username}, you can only gift up to 10 points!";
                        }
                    }
                    else
                    {
                        return $"/me APU @{chatMessage.Username}, the points may not be a comma number and must be positive!";
                    }
                }
            }
            else
            {
                return $"/me APU @{chatMessage.Username}, you need to enter an username and the points you want to gift!";
            }
        }

        public static string SendLocker(ChatMessage chatMessage)
        {
            string result = "";
            if (_database.Dicegamedbs.FirstOrDefault(d => d.UserName == chatMessage.Username).Locker != null)
            {
                string[] ranks = _database.Dicegamedbs.FirstOrDefault(d => d.UserName == chatMessage.Username).Locker.Split();
                ranks.ForEach(d => result += $" {d} |");
                return $"/me APU @{chatMessage.Username}, you already collected these emotes: {result}| [currently equipped: {_database.Dicegamedbs.FirstOrDefault(d => d.UserName == chatMessage.Username).Rank} ]";
            }
            else
            {
                return $"/me APU @{chatMessage.Username}, you have currently no emotes in your locker. If you want to buy an emote use the \"?buy\" command";
            }
        }

        public static string SendPoints(ChatMessage chatMessage)
        {
            if (chatMessage.Message.Split().Length == 1)
            {
                if (_database.Dicegamedbs.Any(d => d.UserName == chatMessage.Username))
                {
                    return $"/me APU [ {_database.Dicegamedbs.FirstOrDefault(r => r.UserName == chatMessage.Username).Rank} ] @{chatMessage.Username} your current points: {_database.Dicegamedbs.FirstOrDefault(d => d.UserName == chatMessage.Username).Points}";
                }
                else
                {
                    return $"/me APU @{chatMessage.Username}, you have currently no points. To earn points you can type \"?Dice\"";
                }
            }
            else
            {
                if (_database.Dicegamedbs.Any(d => d.UserName == chatMessage.Message.Split()[1]))
                {
                    return $"/me APU [ {_database.Dicegamedbs.FirstOrDefault(r => r.UserName == chatMessage.Message.Split()[1]).Rank} ] @{chatMessage.Message.Split()[1]} current points: {_database.Dicegamedbs.FirstOrDefault(d => d.UserName == chatMessage.Message.Split()[1]).Points}";
                }
                else
                {
                    return $"/me APU @{chatMessage.Username}, the user @{chatMessage.Message.Split()[1]} has no points yet.";
                }
            }
        }

        public static string SendShop(ChatMessage chatMessage)
        {
            string result = "";
            JsonController.Ranks.ForEach(r => result += $"{r.Name} No.: {r.EmoteNr}, Price: {r.Price} || ");
            return $"/me APU @{chatMessage.Username}, here is a list of all emotes you can buy and their prices: {result}";
        }

        public static string SendTopPlayers(ChatMessage chatMessage)
        {
            string result = "";
            if (chatMessage.Message.Split().Length >= 2)
            {
                if (_database.Dicegamedbs.Count() < chatMessage.Message.Split()[1].ToInt())
                {
                    List<Dicegamedb> topPlayers = _database.Dicegamedbs.OrderByDescending(d => d.Points).Take(_database.Dicegamedbs.Count()).ToList();
                    topPlayers.ForEach(d => result += $"{d.UserName.Insert(2, "󠀀")}, Points: {d.Points} || ");
                    return $"/me APU @{chatMessage.Username}, the top {_database.Dicegamedbs.Count()}: {result}";
                }
                else
                {

                    if (chatMessage.Message.Split()[1].Equals("all"))
                    {
                        List<Dicegamedb> topPlayers = _database.Dicegamedbs.OrderByDescending(d => d.Points).ToList();
                        topPlayers.ForEach(d => result += $"{d.UserName.Insert(2, "󠀀")}, Points: {d.Points} || ");
                        return $"/me APU @{chatMessage.Username}, the top list: {result}";
                    }
                    else
                    {
                        if (chatMessage.Message.Split()[1].All(char.IsDigit))
                        {
                            if (chatMessage.Message.Split()[1].ToInt() == 1)
                            {
                                Dicegamedb topPlayer = _database.Dicegamedbs.OrderByDescending(d => d.Points).FirstOrDefault();
                                return $"/me APU @{chatMessage.Username}, the best player right now is @{topPlayer.UserName.Insert(2, "󠀀")} with {topPlayer.Points} points B)";
                            }
                            else
                            {
                                List<Dicegamedb> topPlayers = _database.Dicegamedbs.OrderByDescending(d => d.Points).Take(chatMessage.Message.Split()[1].ToInt()).ToList();
                                topPlayers.ForEach(d => result += $"{d.UserName.Insert(2, "󠀀")}, Points: {d.Points} || ");
                                return $"/me APU @{chatMessage.Username}, the top {chatMessage.Message.Split()[1]}: {result}";
                            }
                        }
                        else
                        {
                            return $"/me APU @{chatMessage.Username}, the given number of players may not be a comma number and must be positive!";
                        }
                    }
                }
            }
            else
            {
                List<Dicegamedb> topPlayers = _database.Dicegamedbs.OrderByDescending(d => d.Points).Take(3).ToList();
                topPlayers.ForEach(d => result += $"{d.UserName.Insert(2, "󠀀")}, Points: {d.Points} || ");
                return $"/me APU @{chatMessage.Username}, the top 3: {result}";
            }
        }
    }
}
