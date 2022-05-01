using ApuDoingStuff.Commands.CommandClasses;
using ApuDoingStuff.Commands.DiceGame;
using ApuDoingStuff.Database;
using ApuDoingStuff.Database.Models;
using ApuDoingStuff.Jsons;
using ApuDoingStuff.Utils;
using HLE.Collections;
using HLE.Emojis;
using HLE.Strings;
using HLE.Time;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TwitchLib.Client.Models;
using StringHelper = ApuDoingStuff.Utils.StringHelper;

namespace ApuDoingStuff.Twitch
{
    public static class DiceAction
    {
        private static readonly Regex _accessoiresPattern = new(@"^\?shop\s+(accessoires|acs)", RegexOptions.IgnoreCase | RegexOptions.Compiled, TimeSpan.FromMilliseconds(250));
        private static readonly Regex _rank1Pattern = new(@"^\?shop\s+(rank|emote)s?\s+1", RegexOptions.IgnoreCase | RegexOptions.Compiled, TimeSpan.FromMilliseconds(250));
        private static readonly Regex _rank2Pattern = new(@"^\?shop\s+(rank|emote)s?\s+2", RegexOptions.IgnoreCase | RegexOptions.Compiled, TimeSpan.FromMilliseconds(250));

        public static string GetBigDice(ChatMessage chatMessage, TwitchBot twitchBot)
        {
            BotdbContext database = new();
            Random dice = new();
            Dicegamedb user = DbController.GetFirstOrDefault(chatMessage.Username);
            int randDice = dice.Next(-20, 36);
            if (BigDiceSaveTimer.Timers.Any(d => d.Username == chatMessage.Username))
            {
                return $"/me APU {StringHelper.Rank(chatMessage.Username)}  @{chatMessage.Username}, you can roll your next dice in {TimeHelper.ConvertUnixTimeToTimeStamp(TimeHelper.Now() - (long)BigDiceSaveTimer.Timers.FirstOrDefault(d => d.Username == chatMessage.Username).SaveTimer.RemainingTime)} || [current points of @{chatMessage.Username}: {DbController.GetFirstOrDefault(chatMessage.Username).Points ?? 0}]";
            }
            else
            {
                BigDiceSaveTimer saveTimer = new(chatMessage.Channel, chatMessage.Username, twitchBot);
                BigDiceSaveTimer.Timers.Add(saveTimer);
                if (user != null)
                {
                    DbController.AddPoints(chatMessage.Username, randDice);
                    return $"{GetMessage(chatMessage.Username, randDice)} [current points of @{chatMessage.Username}: {user.Points + randDice}]";
                }
                else
                {
                    return $"/me APU @{chatMessage.Username}, please roll your first dice (\"?dice\") before rolling a big dice!";
                }
            }
        }
        public static string SendBuy(ChatMessage chatMessage)
        {
            if (chatMessage.Message.Split().Length >= 3)
            {
                BotdbContext database = new();
                if (chatMessage.Message.Split()[1] is "emote" or "rank")
                {
                    int emoteNr = chatMessage.Message.Split()[2].ToInt();
                    if (chatMessage.Message.Split()[2].IsMatch(@"^\d+$") && JsonController.Ranks.Any(r => r.EmoteNr == emoteNr))
                    {
                        Dicegamedb user = DbController.GetFirstOrDefault(chatMessage.Username);
                        foreach (Rank r in JsonController.Ranks)
                        {
                            if (r.EmoteNr == emoteNr)
                            {
                                if (!user.EmoteNr.Split().Any(s => s == chatMessage.Message.Split()[2]))
                                {
                                    if (user.Points >= r.Price)
                                    {
                                        _ = user.Rank = r.Name;
                                        DbController.SubPoints(chatMessage.Username, r.Price);
                                        DbController.AddEmoteNr(chatMessage.Username, $" {r.EmoteNr}");
                                        return $"/me APU {Emoji.Tada} congratulations {StringHelper.Rank(chatMessage.Username)} @{chatMessage.Username} you bought the emote: {r.Name}";
                                    }
                                    else
                                    {
                                        return $"/me APU {StringHelper.Rank(chatMessage.Username)} @{chatMessage.Username}, you don't have enough points to buy this emote!";
                                    }
                                }
                                else
                                {
                                    return $"/me APU {StringHelper.Rank(chatMessage.Username)} @{chatMessage.Username}, you already own this emote! (\"?locker\" to get a list of all your emotes)";
                                }
                            }
                        }
                        return string.Empty;
                    }
                    else
                    {
                        return $"/me APU {StringHelper.Rank(chatMessage.Username)} @{chatMessage.Username}, this is not a valid emote number! Type \"?shop\" to see a list of all emotes, emote numbers and their prices.";
                    }
                }
                else
                {
                    if (DbController.GetRank(chatMessage.Username).Any())
                    {
                        int acsNr = chatMessage.Message.Split()[2].ToInt();
                        if (chatMessage.Message.Split()[2].IsMatch(@"^\d+$") && JsonController.Accessoires.Any(r => r.AcsNr == acsNr))
                        {
                            Dicegamedb user = DbController.GetFirstOrDefault(chatMessage.Username);
                            foreach (Accessoires r in JsonController.Accessoires)
                            {
                                if (r.AcsNr == acsNr)
                                {
                                    if (!user.AcsNr?.Split()?.Any(s => s == chatMessage.Message.Split()[2]) == true || string.IsNullOrEmpty(user.AcsNr))
                                    {
                                        if (user.Points >= r.Price)
                                        {
                                            _ = user.Accessoire = r.Name;
                                            DbController.SubPoints(chatMessage.Username, r.Price);
                                            DbController.AddAcsNr(chatMessage.Username, $" {r.AcsNr}");
                                            DbController.SetAcs(chatMessage.Username, Accessoires.GetAccessoire(chatMessage.Message.Split()[2].ToInt()).Name);
                                            return $"/me APU {Emoji.Tada} congratulations {StringHelper.Rank(chatMessage.Username)} @{chatMessage.Username} you bought the accessoire: {r.Name}";
                                        }
                                        else
                                        {
                                            return $"/me APU {StringHelper.Rank(chatMessage.Username)} @{chatMessage.Username}, you don't have enough points to buy this emote!";
                                        }
                                    }
                                    else
                                    {
                                        return $"/me APU {StringHelper.Rank(chatMessage.Username)} @{chatMessage.Username}, you already own this accessoire! (\"?locker acs\") to get a list of all your accessoires)";
                                    }

                                }
                            }
                            return string.Empty;
                        }
                        else
                        {
                            return $"/me APU {StringHelper.Rank(chatMessage.Username)} @{chatMessage.Username}, this is not a valid emote accessoire! Type \"?shop acs\" to see a list of all accessoires and their prices.";
                        }
                    }
                    else
                    {
                        return $"/me APU @{chatMessage.Username}, you need a rank to purchase an accessoire!";
                    }
                }
            }
            else
            {
                return $"/me APU {StringHelper.Rank(chatMessage.Username)} @{chatMessage.Username}, please enter what you would like to buy (emote or accessoire) and the number!";
            }
        }

        public static string SendDice(ChatMessage chatMessage, TwitchBot twitchBot)
        {
            BotdbContext database = new();
            Random dice = new();
            int randDice = dice.Next(1, 7);
            Dicegamedb user = DbController.GetFirstOrDefault(chatMessage.Username);
            if (DiceSaveTimer.Timers.Any(d => d.Username == chatMessage.Username))
            {
                return $"/me APU {StringHelper.Rank(chatMessage.Username)} @{chatMessage.Username}, you can roll your next dice in {TimeHelper.ConvertUnixTimeToTimeStamp(TimeHelper.Now() - (long)DiceSaveTimer.Timers.FirstOrDefault(d => d.Username == chatMessage.Username).SaveTimer.RemainingTime)} || [current points of @{chatMessage.Username}: {DbController.GetFirstOrDefault(chatMessage.Username).Points ?? 0}]";
            }
            else
            {
                DiceSaveTimer saveTimer = new(chatMessage.Channel, chatMessage.Username, twitchBot);
                DiceSaveTimer.Timers.Add(saveTimer);
                if (user != null)
                {
                    DbController.AddPoints(chatMessage.Username, randDice);
                    return $"/me APU {StringHelper.Rank(chatMessage.Username)} @{chatMessage.Username}, you got {randDice} {Emoji.GameDie} [current points: {user.Points + randDice}]";
                }
                else
                {
                    _ = database.Dicegamedbs.Add(new Dicegamedb { UserName = chatMessage.Username, Points = randDice, PingMe = true, Rank = "-" });
                    database.SaveChanges();
                    return $"/me APU @{chatMessage.Username}, you got {randDice}";
                }
            }
        }

        public static string SendDiceFight(ChatMessage chatMessage, TwitchBot twitchBot)
        {
            BotdbContext database = new();
            Dicegamedb user = DbController.GetFirstOrDefault(chatMessage.Username);
            if (chatMessage.Message.Split().Length >= 3)
            {
                string points = chatMessage.Message.Split()[2];
                if (points.All(char.IsDigit) && points.ToInt() > 0)
                {
                    int setPoints = chatMessage.Message.Split()[2].ToInt();
                    int winnerPoints = setPoints + setPoints;
                    if (TwitchBot.FightAccepts.Any(d => d.Opponent == chatMessage.Message.Split()[1]))
                    {
                        return $"/me APU [ {user.Rank} ] @{chatMessage.Username}, this user is already in a fight, please wait until the fight is over.";
                    }
                    else
                    {
                        if (DbController.GetFirstOrDefault(chatMessage.Message.Split()[1]) != null)
                        {
                            if (chatMessage.Message.Split()[1].ToLower() == chatMessage.Username)
                            {
                                return $"/me APU you can't fight yourself {StringHelper.Rank(chatMessage.Username)} @{chatMessage.Username}";
                            }
                            else
                            {
                                string enemy = chatMessage.Message.Split()[1].ToLower();
                                Dicegamedb enemyDb = DbController.GetFirstOrDefault(enemy);
                                if (user.Points >= setPoints)
                                {
                                    if (enemyDb != null && enemyDb.Points >= setPoints)
                                    {
                                        TwitchBot.FightAccepts.Add(new(enemy, chatMessage.Channel, setPoints, chatMessage.Username));
                                        FightSaveTimer saveTimer = new(chatMessage.Channel, enemy, chatMessage.Username, twitchBot);
                                        TwitchBot.FightSaveTimers.Add(saveTimer);
                                        return $"/me APU {StringHelper.Rank(enemy)} @{enemy}, you were challenged. If you want to accept please type \"?fight accept\" otherwise type \"?fight decline\"";
                                    }
                                    else
                                    {
                                        return $"/me APU {StringHelper.Rank(chatMessage.Username)} @{chatMessage.Username}, your opponent has not enough points to compete [current points of @{enemy}: {enemyDb.Points}]";
                                    }
                                }
                                else
                                {
                                    return $"/me APU {StringHelper.Rank(chatMessage.Username)} @{chatMessage.Username}, you don't have enough points to compete [current points of @{chatMessage.Username}: {user.Points}]";
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
                    return $"/me APU {StringHelper.Rank(chatMessage.Username)} @{chatMessage.Username}, the set points may not be a comma number and must be positive!";
                }
            }
            else
            {
                return $"/me APU {StringHelper.Rank(chatMessage.Username)} @{chatMessage.Username}, to fight against an user you need to type the username of your opponent and the points you would like to set. (eg. \"?DiceFight forsen 213\")";
            }
        }

        public static string SendEquip(ChatMessage chatMessage)
        {
            BotdbContext database = new();
            Dicegamedb user = DbController.GetFirstOrDefault(chatMessage.Username);
            if (chatMessage.Message.Split().Length >= 3)
            {
                string result = string.Empty;
                switch (chatMessage.Message.Split()[1])
                {
                    case "emote" or "rank":
                        result = $"/me APU {StringHelper.Rank(chatMessage.Username)} @{chatMessage.Username}, this rank is invalid!";
                        Rank rank = Rank.GetRank(chatMessage.Message.Split()[2]);
                        if (rank != null && user.EmoteNr.Split().Any(s => s == rank.EmoteNr.ToString()))
                        {
                            if (chatMessage.Message.Split()[1] != user.Rank)
                            {
                                DbController.SetRank(chatMessage.Username, chatMessage.Message.Split()[2]);
                                result = $"/me APU {StringHelper.Rank(chatMessage.Username)} @{chatMessage.Username}, you succesfully equipped: {chatMessage.Message.Split()[2]}";
                            }
                            else
                            {
                                result = $"/me APU {StringHelper.Rank(chatMessage.Username)} @{chatMessage.Username}, you currently have this emote equipped!";
                            }
                        }
                        else
                        {
                            if (chatMessage.Message.Split()[2].All(char.IsDigit))
                            {
                                int emoteNr = chatMessage.Message.Split()[2].ToInt();
                                if (user.EmoteNr.Split().Any(s => s == chatMessage.Message.Split()[2]))
                                {
                                    if (JsonController.Ranks.Any(r => r.EmoteNr == emoteNr))
                                    {
                                        foreach (Rank r in JsonController.Ranks)
                                        {
                                            if (r.EmoteNr == emoteNr)
                                            {
                                                DbController.SetRank(chatMessage.Username, r.Name);
                                                result = $"/me APU {StringHelper.Rank(chatMessage.Username)} @{chatMessage.Username}, you succesfully equipped: {r.Name}";
                                            }
                                        }
                                    }
                                    else
                                    {
                                        result = $"/me APU {StringHelper.Rank(chatMessage.Username)} @{chatMessage.Username}, this rank doesn't exists!";
                                    }
                                }
                                else
                                {
                                    result = $"/me APU {StringHelper.Rank(chatMessage.Username)} @{chatMessage.Username}, you do not have this emote yet or this rank doesn't exists!";
                                }
                            }
                        }
                        break;
                    case "acs" or "accessoire":
                        result = $"/me APU {StringHelper.Rank(chatMessage.Username)} @{chatMessage.Username}, this accessoire is invalid!";
                        Accessoires acs = Accessoires.GetAccessoire(chatMessage.Message.Split()[2]);
                        if (acs != null)
                        {
                            if (chatMessage.Message.Split()[2].All(char.IsDigit))
                            {
                                acs = Accessoires.GetAccessoire(chatMessage.Message.Split()[2].ToInt());
                            }
                        }
                        if (user.AcsNr.Split().Any(d => d == acs.AcsNr.ToString()))
                        {
                            if (chatMessage.Message.Split()[1] != user.Accessoire)
                            {
                                DbController.SetAcs(chatMessage.Username, chatMessage.Message.Split()[2]);
                                if (acs.IsZeroWidth)
                                {
                                    result = $"/me APU {StringHelper.Rank(chatMessage.Username)} @{chatMessage.Username}, you succesfully equipped: ⠀⠀⠀ {chatMessage.Message.Split()[2]}";
                                }
                                else
                                {
                                    result = $"/me APU {StringHelper.Rank(chatMessage.Username)} @{chatMessage.Username}, you succesfully equipped: {chatMessage.Message.Split()[2]}";
                                }

                            }
                            else
                            {
                                result = $"/me APU {StringHelper.Rank(chatMessage.Username)} @{chatMessage.Username}, you currently have this emote equipped!";
                            }
                        }
                        else
                        {
                            int acsNr = chatMessage.Message.Split()[2].ToInt();
                            if (user.AcsNr.Split().Any(s => s == chatMessage.Message.Split()[2]))
                            {
                                if (Accessoires.GetAccessoire(acsNr) != null)
                                {
                                    foreach (Accessoires r in JsonController.Accessoires)
                                    {
                                        if (r.AcsNr == acsNr)
                                        {
                                            DbController.SetAcs(chatMessage.Username, r.Name);
                                            result = $"/me APU {StringHelper.Rank(chatMessage.Username)} @{chatMessage.Username}, you succesfully equipped: {r.Name}";
                                        }
                                    }
                                }
                                else
                                {
                                    result = $"/me APU {StringHelper.Rank(chatMessage.Username)} @{chatMessage.Username}, this accessoire doesn't exists!";
                                }
                            }
                            else
                            {
                                result = $"/me APU {StringHelper.Rank(chatMessage.Username)} @{chatMessage.Username}, you do not have this accessoire yet or this accessoire doesn't exists!";
                            }
                        }

                        break;
                }
                return result;
            }
            else
            {
                return $"/me APU {StringHelper.Rank(chatMessage.Username)} @{chatMessage.Username}, you need to enter the type you want to equip and the emote/accessoire itself!";
            }
        }

        public static string SendFight(ChatMessage chatMessage)
        {
            BotdbContext database = new();
            Dicegamedb user = DbController.GetFirstOrDefault(chatMessage.Username);
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
                    DbController.SubPoints(challenger, points);
                    DbController.SubPoints(chatMessage.Username, points);
                    DbController.AddPoints(winner, winnerPoints);
                    TwitchBot.FightSaveTimers.FirstOrDefault(d => d.Username == chatMessage.Username).FightTimer.Stop();
                    TwitchBot.FightSaveTimers.Remove(TwitchBot.FightSaveTimers.FirstOrDefault(d => d.Username == chatMessage.Username));
                    TwitchBot.FightAccepts.Remove(TwitchBot.FightAccepts.FirstOrDefault(d => d.Opponent == chatMessage.Username));
                    if (winner == challenger)
                    {
                        if (points == 1)
                        {
                            return $"/me APU {Emoji.ConfettiBall} PAAG @{winner} you won against @{chatMessage.Username} and made {winnerPoints} points! Unfortunately you lost {points} point @{chatMessage.Username} ... sooo unlucky :/ ";
                        }
                        else
                        {
                            return $"/me APU {Emoji.ConfettiBall} PAAG @{winner} you won against @{chatMessage.Username} and made {winnerPoints} points! Unfortunately you lost {points} points @{chatMessage.Username} ... sooo unlucky :/ ";
                        }
                    }
                    else if (winner == chatMessage.Username)
                    {
                        if (points == 1)
                        {
                            return $"/me APU {Emoji.ConfettiBall} PAAG @{winner} you won against @{challenger} and made {winnerPoints} points! Unfortunately you lost {points} point @{challenger} ... sooo unlucky :/ ";
                        }
                        else
                        {
                            return $"/me APU {Emoji.ConfettiBall} PAAG @{winner} you won against @{challenger} and made {winnerPoints} points! Unfortunately you lost {points} points @{challenger} ... sooo unlucky :/ ";
                        }
                    }
                }

                if (chatMessage.Message.IsMatch("^?fight decline"))
                {
                    TwitchBot.FightAccepts.FirstOrDefault(d => d.Accepted = false);
                    TwitchBot.FightAccepts.Remove(TwitchBot.FightAccepts.FirstOrDefault(d => d.Opponent == chatMessage.Username));
                    return $"/me APU {StringHelper.Rank(chatMessage.Username)} @{challenger}, your opponent denied the fight.";
                }
                else
                {
                    return string.Empty;
                }
            }
            else
            {
                return $"/me APU {StringHelper.Rank(chatMessage.Username)} @{chatMessage.Username}, you have not been challenged.";
            }
        }

        public static string SendGift(ChatMessage chatMessage)
        {
            BotdbContext database = new();
            Dicegamedb user = DbController.GetFirstOrDefault(chatMessage.Username);
            Dicegamedb split1 = DbController.GetFirstOrDefault(chatMessage.Message.Split()[1]);
            if (chatMessage.Message.Split().Length == 3)
            {
                if (chatMessage.Message.Split()[1].ToLower() == chatMessage.Username)
                {
                    return $"/me APU {StringHelper.Rank(chatMessage.Username)} @{chatMessage.Username}, you can't gift yourself points!";
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
                                if (user != null && user.Points >= 0)
                                {
                                    if (chatMessage.Message.Split()[1] != "random")
                                    {
                                        DbController.SubPoints(chatMessage.Username, points);
                                        DbController.AddPoints(chatMessage.Message.Split()[1], points);
                                        return $"/me APU {StringHelper.Rank(chatMessage.Username)} @{chatMessage.Username} you gifted @{chatMessage.Message.Split()[1]} {points} point!";
                                    }
                                    else
                                    {
                                        string randUser = database.Dicegamedbs.ToList().Random().UserName;
                                        DbController.SubPoints(chatMessage.Username, points);
                                        DbController.AddPoints(randUser, points);
                                        return $"/me APU {StringHelper.Rank(chatMessage.Username)} @{chatMessage.Username} you gifted @{randUser} {points} point!";
                                    }
                                }
                                else
                                {
                                    return $"/me APU it appears that @{chatMessage.Username} has no points yet. Type \"?Dice\" to earn points.";
                                }
                            }
                            else
                            {
                                if (user != null && user.Points >= 0)
                                {
                                    if (chatMessage.Message.Split()[1] != "random")
                                    {
                                        DbController.SubPoints(chatMessage.Username, points);
                                        DbController.AddPoints(chatMessage.Message.Split()[1], points);
                                        return $"/me APU {StringHelper.Rank(chatMessage.Username)} @{chatMessage.Username} you gifted @{chatMessage.Message.Split()[1]} {points} point!";
                                    }
                                    else
                                    {
                                        string randUser = database.Dicegamedbs.ToList().Random().UserName;
                                        DbController.SubPoints(chatMessage.Username, points);
                                        DbController.AddPoints(randUser, points);
                                        return $"/me APU {StringHelper.Rank(chatMessage.Username)} @{chatMessage.Username} you gifted @{randUser} {points} point!";
                                    }
                                }
                                else
                                {
                                    return $"/me APU it appears that @{chatMessage.Username} has no points yet. Type \"?Dice\" to earn points.";
                                }
                            }
                        }
                        else
                        {
                            return $"/me APU {StringHelper.Rank(chatMessage.Username)} @{chatMessage.Username}, you can only gift up to 10 points!";
                        }
                    }
                    else
                    {
                        return $"/me APU {StringHelper.Rank(chatMessage.Username)} @{chatMessage.Username}, the points may not be a comma number and must be positive!";
                    }
                }
            }
            else
            {
                return $"/me APU {StringHelper.Rank(chatMessage.Username)} @{chatMessage.Username}, you need to enter an username and the points you want to gift!";
            }
        }

        public static string SendLocker(ChatMessage chatMessage)
        {
            Dicegamedb user = DbController.GetFirstOrDefault(chatMessage.Username);
            if (user != null)
            {
                string result = string.Empty;
                if (chatMessage.Message.Split().Length >= 2)
                {
                    string returnResult = string.Empty;
                    switch (chatMessage.Message.Split()[1])
                    {
                        case "accessoires" or "acs":
                            if (user.AcsNr != null && user.AcsNr != string.Empty)
                            {
                                Accessoires[] acs = Accessoires.GetAccessoire(user.AcsNr.Split());
                                acs.ForEach(d => result += $" {d.Name} |");
                                returnResult = $"/me APU {StringHelper.Rank(chatMessage.Username)} @{chatMessage.Username}, you already collected these accessoires: {result}| [currently equipped: {user.Rank} ]";
                            }
                            else
                            {
                                returnResult = $"/me APU @{chatMessage.Username}, you have currently no accessoires in your locker. If you want to buy an emote use the \"?buy\" command";
                            }
                            break;
                        case "emotes" or "ranks" or null or not "emotes" or not "ranks" or not "accessoires" or not "acs":
                            if (user.EmoteNr != null && user.EmoteNr != string.Empty)
                            {
                                Rank[] ranks = Rank.GetRank(user.EmoteNr.Split());
                                ranks.ForEach(d => result += $" {d.Name} |");
                                returnResult = $"/me APU {StringHelper.Rank(chatMessage.Username)} @{chatMessage.Username}, you already collected these emotes: {result}| [currently equipped: {user.Rank} ]";
                            }
                            else
                            {
                                returnResult = $"/me APU @{chatMessage.Username}, you have currently no emotes in your locker. If you want to buy an emote use the \"?buy emotes\" command";
                            }
                            break;
                    }
                    return returnResult;
                }
                else
                {
                    if (user.EmoteNr != null && user.EmoteNr != string.Empty)
                    {
                        Rank[] ranks = Rank.GetRank(user.EmoteNr.Split());
                        ranks.ForEach(d => result += $" {d.Name} |");
                        return $"/me APU {StringHelper.Rank(chatMessage.Username)} @{chatMessage.Username}, you already collected these emotes: {result}| [currently equipped: {user.Rank} ]";
                    }
                    else
                    {
                        return $"/me APU @{chatMessage.Username}, you have currently no emotes in your locker. If you want to buy an emote use the \"?buy emotes\" command";
                    }

                }
            }
            else
            {
                return $"/me APU @{chatMessage.Username}, you have currently no emotes in your locker. If you want to buy an emote you need to earn points (\"?Dice\")!";
            }
        }

        public static string SendPoints(ChatMessage chatMessage)
        {
            Dicegamedb user = DbController.GetFirstOrDefault(chatMessage.Username);
            if (chatMessage.Message.Split().Length == 1)
            {
                if (user != null)
                {
                    return $"/me APU {StringHelper.Rank(chatMessage.Username)} @{chatMessage.Username} your current points: {user.Points}";
                }
                else
                {
                    return $"/me APU @{chatMessage.Username}, you have currently no points. To earn points you can type \"?Dice\"";
                }
            }
            else
            {
                Dicegamedb split1 = DbController.GetFirstOrDefault(chatMessage.Message.Split()[1]);
                if (split1 != null)
                {
                    return $"/me APU {StringHelper.Rank(chatMessage.Message.Split()[1])} @{chatMessage.Message.Split()[1]} current points: {DbController.GetFirstOrDefault(chatMessage.Message.Split()[1]).Points}";
                }
                else
                {
                    return $"/me APU @{chatMessage.Username}, the user @{chatMessage.Message.Split()[1]} has no points yet.";
                }
            }
        }

        public static string SendShop(ChatMessage chatMessage)
        {
            string result = string.Empty;
            if (_accessoiresPattern.IsMatch(chatMessage.Message))
            {
                JsonController.Accessoires.ForEach(r =>
                {
                    string name = r.Name;
                    if (r.IsZeroWidth)
                    {
                        name = $"⠀⠀⠀ {name}";
                    }
                    result += $"{name} No.: {r.AcsNr}, Price: {r.Price} || ";

                });
                return $"/me APU @{chatMessage.Username}, here are all accessoires you can buy and their prices: {result}";
            }
            else if (_rank2Pattern.IsMatch(chatMessage.Message))
            {
                JsonController.Ranks.Skip(6).ForEach(r => result += $"{r.Name} No.: {r.EmoteNr}, Price: {r.Price} || ");
                return $"/me APU @{chatMessage.Username}, here is the second page of emotes you can buy and their prices: {result}";
            }
            else
            {
                JsonController.Ranks.Take(6).ForEach(r => result += $"{r.Name} No.: {r.EmoteNr}, Price: {r.Price} || ");
                return $"/me APU @{chatMessage.Username}, here is the first page of emotes you can buy and their prices: {result}";
            }
        }

        public static string SendTopPlayers(ChatMessage chatMessage)
        {
            BotdbContext database = new();
            string result = "";
            if (chatMessage.Message.Split().Length >= 2)
            {
                string split1 = chatMessage.Message.Split()[1];
                if (split1.Equals("all") || split1.All(char.IsDigit))
                {
                    if (split1.Equals("all"))
                    {
                        List<Dicegamedb> topPlayers = database.Dicegamedbs.OrderByDescending(d => d.Points).ToList();
                        topPlayers.ForEach(d => result += $"{d.UserName.Antiping()}, Points: {d.Points} || ");
                        return $"/me APU @{chatMessage.Username}, the top list: {result}";
                    }
                    else
                    {
                        if (database.Dicegamedbs.Count() < split1.ToInt())
                        {
                            List<Dicegamedb> topPlayers = database.Dicegamedbs.OrderByDescending(d => d.Points).Take(database.Dicegamedbs.Count()).ToList();
                            topPlayers.ForEach(d => result += $"{d.UserName.Insert(2, "󠀀")}, Points: {d.Points} || ");
                            return $"/me APU @{chatMessage.Username}, the top {database.Dicegamedbs.Count()}: {result}";
                        }
                        else
                        {
                            if (split1.ToInt() == 1)
                            {
                                Dicegamedb topPlayer = database.Dicegamedbs.OrderByDescending(d => d.Points).FirstOrDefault();
                                return $"/me APU @{chatMessage.Username}, the best player right now is @{topPlayer.UserName.Insert(2, "󠀀")} with {topPlayer.Points} points B)";
                            }
                            else
                            {
                                List<Dicegamedb> topPlayers = database.Dicegamedbs.OrderByDescending(d => d.Points).Take(chatMessage.Message.Split()[1].ToInt()).ToList();
                                topPlayers.ForEach(d => result += $"{d.UserName.Insert(2, "󠀀")}, Points: {d.Points} || ");
                                return $"/me APU @{chatMessage.Username}, the top {chatMessage.Message.Split()[1]}: {result}";
                            }
                        }
                    }

                }
                else
                {
                    return $"/me APU @{chatMessage.Username}, the given number of players may not be a comma number and must be positive or with \"?top all\" display all player!";
                }
            }
            else
            {
                List<Dicegamedb> topPlayers = database.Dicegamedbs.OrderByDescending(d => d.Points).Take(3).ToList();
                topPlayers.ForEach(d => result += $"{d.UserName.Insert(2, "󠀀")}, Points: {d.Points} || ");
                return $"/me APU @{chatMessage.Username}, the top 3: {result}";
            }
        }

        private static string GetMessage(string username, int randDice)
        {
            switch (randDice)
            {
                case >= 30: return $"/me APU {Emoji.ConfettiBall} WOAAAHHH APUUUU CONGRATS {StringHelper.Rank(username)} @{username.ToUpper()} YOU GOT {randDice}!!!";
                case >= 20: return $"/me APU {Emoji.PointRight}{Emoji.PointLeft} {StringHelper.Rank(username)} @{username} h...here is {randDice} for you.";
                case >= 10: return $"/me APU {Emoji.MagicWand} {Emoji.Sparkles} {StringHelper.Rank(username)} @{username} the great apu wizard gives you a well deserved {randDice}! {Emoji.Sparkles}";
                case > 0: return $"/me FBPass APU FBBlock RUUUNNNN {StringHelper.Rank(username)} @{username.ToUpper()} OR YOU WILL MISS YOUR {randDice}!";
                case < -10: return $"/me APU {StringHelper.Rank(username)} @{username} oh well that's sad ... you got {randDice} :(";
                case <= 0: return $"/me APU {StringHelper.Rank(username)} @{username} sooo unlucky you got {randDice}! :/";
            }
        }
    }
}
