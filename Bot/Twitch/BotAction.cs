using ApuDoingStuff.API;
using ApuDoingStuff.Commands;
using ApuDoingStuff.Commands.CommandClasses.Lists;
using ApuDoingStuff.Commands.CommandClasses.Timer;
using ApuDoingStuff.Database;
using ApuDoingStuff.Database.Models;
using ApuDoingStuff.Jsons;
using ApuDoingStuff.Properties;
using ApuDoingStuff.Utils;
using HLE.Collections;
using HLE.Emojis;
using HLE.Time;
using System.Collections.Generic;
using System.Linq;
using TwitchLib.Client.Models;

namespace ApuDoingStuff.Twitch
{
    public static class BotAction
    {
        private static readonly BotdbContext _database = new();

        public static void AddCooldown(string username, CommandType type)
        {
            if (TwitchBot.Cooldowns.Any(c => c.Username == username && c.Type == type))
            {
                TwitchBot.Cooldowns.Remove(
                    TwitchBot.Cooldowns.FirstOrDefault(c => c.Username == username && c.Type == type)
                );
                AddUserToCooldownDictionary(username, type);
            }
        }

        public static void AddMessageCooldown(string username, MessageType type)
        {
            if (TwitchBot.MessageCooldowns.Any(c => c.Username == username && c.Type == type))
            {
                TwitchBot.MessageCooldowns.Remove(
                    TwitchBot.MessageCooldowns.FirstOrDefault(c => c.Username == username && c.Type == type)
                );
                AddUserToMessageCooldownDictionary(username, type);
            }
        }

        public static void AddUserToCooldownDictionary(string username, CommandType type)
        {
            if (!TwitchBot.Cooldowns.Any(c => c.Username == username && c.Type == type))
            {
                TwitchBot.Cooldowns.Add(new Cooldown(username, type));
            }
        }

        public static void AddUserToMessageCooldownDictionary(string username, MessageType type)
        {
            if (!TwitchBot.MessageCooldowns.Any(c => c.Username == username && c.Type == type))
            {
                TwitchBot.MessageCooldowns.Add(new(username, type));
            }
        }

        public static string GetCommands(ChatMessage chatMessage)
        {
            return $"/me APU @{chatMessage.Username}, here you can find all commands: https://benastro.github.io/ApuDoingStuff/";
        }

        public static string GetHelp(ChatMessage chatMessage)
        {
            return $"/me APU @{chatMessage.Username}, here you can find all commands: https://benastro.github.io/ApuDoingStuff/";
        }

        public static string GetGitHub(ChatMessage chatMessage)
        {
            return $"/me APU @{chatMessage.Username}, here you can find the repository of the bot https://github.com/benASTRO/ApuDoingStuff";
        }

        public static string GetRacc(ChatMessage chatMessage)
        {
            return $"/me APU @{chatMessage.Username}, here you can find all kinds of information about @{chatMessage.Message.Split()[1].ToLower()}: https://emotes.raccatta.cc/twitch/{chatMessage.Message.Split()[1].ToLower()} RaccAttack";
        }

        public static string GetSuggestion(ChatMessage chatMessage)
        {
            BotdbContext database = new();
            if (chatMessage.Message.Split().Length >= 2)
            {
                database.Suggestions.Add(new Suggestion { Username = chatMessage.Username, Suggestion1 = string.Join(" ", chatMessage.Message.Split()[1..]) });
                database.SaveChanges();
                return $"/me APU @{chatMessage.Username}, the suggestion has been noted, thank you!";
            }
            else
            {
                return $"/me APU @{chatMessage.Username}, you need to add a suggestion to your message.";
            }
        }

        public static bool IsOnCooldown(string username, CommandType type)
        {
            return TwitchBot.Cooldowns.Any(c => c.Username == username && c.Type == type && c.Time > TimeHelper.Now());
        }
        public static bool IsOnMessageCooldown(string username, MessageType type)
        {
            return TwitchBot.MessageCooldowns.Any(c => c.Username == username && c.Type == type && c.Time > TimeHelper.Now());
        }
        public static string SendApuPic(ChatMessage chatMessage)
        {
            return $"/me APU @{chatMessage.Username}, {HttpRequest.RandomApuTitleUrl()} {HttpRequest.RandomApuPicUrl()}";
        }

        public static string SendAxolotl(ChatMessage chatMessage)
        {
            return $"/me APU @{chatMessage.Username}, {HttpRequest.RandomAxolotFact()} {HttpRequest.RandomAxolotlUrl()}";
        }

        public static string SendCat(ChatMessage chatMessage)
        {
            return $"/me APU @{chatMessage.Username}, {HttpRequest.RandomCatFact()} {HttpRequest.RandomCatUrl()} CoolCat";
        }

        public static string SendDog(ChatMessage chatMessage)
        {
            return $"/me APU @{chatMessage.Username}, {HttpRequest.RandomDogUrl()} Wowee";
        }

        public static string SendDuck(ChatMessage chatMessage)
        {
            return $"/me APU @{chatMessage.Username}, {HttpRequest.RandomDuckUrl()} DuckerZ";
        }

        public static string SendJoin(ChatMessage chatMessage, TwitchBot twitchBot)
        {
            if (chatMessage.Message.Split().Length == 2)
            {
                string channel = chatMessage.Message.Split()[1];

                if (chatMessage.Username == Resources.Owner)
                {
                    _ = _database.Channels.Add(new Database.Models.Channel { Channel1 = $"{channel}" });
                    if (_database.Channels.Any(d => d.Channel1 == channel))
                    {
                        return $"/me APU @{chatMessage.Username}, the bot is already connected to this channel!";
                    }
                    else
                    {
                        twitchBot.TwitchClient.JoinChannel(chatMessage.Message.Split()[1]);
                        _database.SaveChanges();
                        twitchBot.Send(chatMessage.Message.Split()[1], $"/me APU {Emoji.ConfettiBall} bot joined!");
                        return $"/me APU Bot joined channel: @{channel}";

                    }
                }
                else
                {
                    return $"/me APU no, i don't think so @{chatMessage.Username} [owner only]";
                }
            }
            else
            {
                return $"/me APU @{chatMessage.Username}, this is an invalid username";
            }
        }
        public static string SendLeave(ChatMessage chatMessage, TwitchBot twitchBot)
        {
            if (chatMessage.IsModerator || chatMessage.IsBroadcaster || chatMessage.Username == Resources.Owner)
            {
                twitchBot.TwitchClient.LeaveChannel(chatMessage.Channel);
                _database.Channels.Remove(_database.Channels.FirstOrDefault(d => d.Channel1 == chatMessage.Channel));
                _database.SaveChanges();
                return $"/me APU {Emoji.Wave} bye bye";
            }
            else
            {
                return $"/me APU no, i don't think so @{chatMessage.Username} [moderator/broadcaster only]";
            }
        }

        public static string SendLyrics(ChatMessage chatMessage)
        {
            string title = chatMessage.Message.Split()[1..].ToSequence();
            string result = HttpRequest.LyricsUrl(title);
            return $"/me APU @{chatMessage.Username}, {result}";
        }

        public static string SendPing(ChatMessage chatMessage, TwitchBot twitchBot)
        {
            return $"/me APU {Emoji.MagicWand} PONG! {twitchBot.GetSystemInfo()}";
        }
        public static string SendShiba(ChatMessage chatMessage)
        {
            return $"/me APU @{chatMessage.Username}, {HttpRequest.RandomShibaUrl()} ConcernDoge";
        }

        public static string SendShuffle(ChatMessage chatMessage)
        {
            string result = string.Empty;
            if (chatMessage.Message.Length < 2)
            {
                List<string> words = chatMessage.Message.Split(" ")[1..].ToList();
                int length = words.Count;
                for (int i = 0; i < length; i++)
                {
                    string word = words.Random();
                    result += $"{word} ";
                    words.Remove(word);
                }
            }
            else
            {
                List<char> chars = chatMessage.Message[9..].ToList();
                int length = chars.Count;
                for (int i = 0; i < length; i++)
                {
                    char letter = chars.Random();
                    result += $"{letter}";
                    chars.Remove(letter);
                }
            }
            return $"/me {result}";
        }

        public static string GetChannels(ChatMessage chatMessage)
        {
            BotdbContext database = new();
            return $"/me APU @{chatMessage.Username}, these are all channel i'm connected to: {string.Join(", ", database.Channels.Where(d => d.Channel1 != Resources.pajaShh.Split()[0] && d.Channel1 != "ApuDoingStuff").Select(d => d.Channel1.Antiping()))}";
        }

        private static string GetWord()
        {
            string word = JsonController.Words.Where(d => d.Length == 5).Random();
            return word;
        }

        public static void GetProcess(ChatMessage chatMessage, ref string result)
        {
            WordleGame game = TwitchBot.WordleGames.FirstOrDefault(d => d.Username == chatMessage.Username);
            string word = ListController.GetWord(chatMessage.Username);
            for (int i = 0; i < word.Length; i++)
            {
                if (word[i] == chatMessage.Message.Split()[1][i])
                {
                    int counter = 0;
                    int counter2 = 0;
                    counter = word.Count(d => d == word[i]);
                    counter2 = chatMessage.Message.Split()[1].Count(d => d == chatMessage.Message.Split()[1][i]);
                    if (counter == counter2)
                    {
                        game.RightLetters[i] = chatMessage.Message.Split()[1][i];
                        result += $"{Emoji.GreenSquare} {char.ToUpper(game.RightLetters[i]).ZeroWidth()}";
                    }
                    else if (counter < counter2)
                    {
                        for (int j = 0; j < word.Length; j++)
                        {
                            if (word[i] == chatMessage.Message.Split()[1][j])
                            {
                                game.RightLetters[i] = chatMessage.Message.Split()[1][i];
                                result += $"{Emoji.GreenSquare} {char.ToUpper(game.RightLetters[i]).ZeroWidth()}";
                            }
                            else
                            {
                                if (game.FalseLetters.Any(d => d != chatMessage.Message.Split()[1][i]))
                                {
                                    game.FalseLetters.Add(chatMessage.Message.Split()[1][i]);
                                }
                                result += Emoji.BlackLargeSquare;
                            }
                        }
                    }
                    else
                    {

                    }
                }
                else if (word.Contains(chatMessage.Message.Split()[1][i]))
                {
                    if (game.PlacementLetters.Any(d => d != chatMessage.Message.Split()[1][i]))
                    {
                        game.PlacementLetters[i] = chatMessage.Message.Split()[1][i];
                    }
                    result += $"{Emoji.OrangeSquare} {char.ToUpper(chatMessage.Message.Split()[1][i]).ZeroWidth()}";
                }
                else
                {
                    if (!game.FalseLetters.Contains(chatMessage.Message.Split()[1][i]))
                    {
                        game.FalseLetters.Add(chatMessage.Message.Split()[1][i]);
                    }
                    result += Emoji.BlackLargeSquare;
                }
            }
        }

        public static string SendWordle(ChatMessage chatMessage, TwitchBot twitchBot)
        {
            if (WordleTimer.Timers.All(d => d.Username != chatMessage.Username))
            {

                if (chatMessage.Message.Split().Length == 1)
                {
                    if (!ListController.DoesGameExists(chatMessage.Username))
                    {
                        WordleGame wList = new(chatMessage.Username, GetWord(), chatMessage.Channel);
                        TwitchBot.WordleGames.Add(wList);
                        return $"/me APU @{chatMessage.Username}, you have 6 chances to guess a 5 letter long word. With \"?wordle process\" you can see which letters you've used so far. :D";
                    }
                    else
                    {
                        return $"/me APU @{chatMessage.Username}, you're already in a game!";
                    }
                }
                else if (chatMessage.Message.Split()[1] == "process")
                {
                    if (ListController.DoesGameExists(chatMessage.Username))
                    {
                        if (TwitchBot.WordleGames.FirstOrDefault(d => d.Username == chatMessage.Username).Channel == chatMessage.Channel)
                        {
                            string word = ListController.GetWord(chatMessage.Username);
                            return $"/me APU @{chatMessage.Username}, {ListController.GetProcess(chatMessage.Username, word).ToUpper()} || False Placement: {ListController.GetPlacementLetters(chatMessage.Username).ToUpper()} || False letters: {ListController.GetFalseLetters(chatMessage.Username).ToUpper()} || Tries left: {ListController.GetTries(chatMessage.Username)}";
                        }
                        else
                        {
                            return string.Empty;
                        }
                    }
                    else
                    {
                        return $"/me APU @{chatMessage.Username}, you're currently not in a game!";
                    }
                }
                else
                {
                    if (ListController.DoesGameExists(chatMessage.Username))
                    {
                        if (TwitchBot.WordleGames.FirstOrDefault(d => d.Username == chatMessage.Username).Channel == chatMessage.Channel)
                        {
                            if (chatMessage.Message.Split()[1].Length == 5)
                            {
                                if (TwitchBot.WordleGames.FirstOrDefault(d => d.Username == chatMessage.Username).Tries > 0)
                                {
                                    TwitchBot.WordleGames.FirstOrDefault(d => d.Username == chatMessage.Username).Tries--;
                                    string word = ListController.GetWord(chatMessage.Username);
                                    if (word != chatMessage.Message.Split()[1])
                                    {
                                        string result = string.Empty;
                                        GetProcess(chatMessage, ref result);
                                        return $" APU @{chatMessage.Username}, {result} || Tries: {ListController.GetTries(chatMessage.Username)}";
                                    }
                                    else
                                    {
                                        string result = $"/me APU @{chatMessage.Username}, congratulations you've solved it! VisLaud You earned 50 points.";
                                        DbController.AddPoints(chatMessage.Username, 50);
                                        TwitchBot.WordleGames.Remove(TwitchBot.WordleGames.FirstOrDefault(d => d.Username == chatMessage.Username));
                                        WordleTimer saveTimer = new(chatMessage.Channel, chatMessage.Username, twitchBot);
                                        WordleTimer.Timers.Add(saveTimer);
                                        return result;
                                    }
                                }
                                else
                                {
                                    string word = ListController.GetWord(chatMessage.Username);
                                    if (word != chatMessage.Message.Split()[1])
                                    {
                                        WordleGame game = TwitchBot.WordleGames.FirstOrDefault(d => d.Username == chatMessage.Username);
                                        string result = string.Empty;
                                        for (int i = 0; i < word.Length; i++)
                                        {
                                            if (word[i] == chatMessage.Message.Split()[1][i])
                                            {
                                                game.RightLetters[i] = chatMessage.Message.Split()[1][i];
                                                result += $"{Emoji.GreenSquare} {char.ToUpper(game.RightLetters[i]).ZeroWidth()}";
                                            }
                                            else if (word.Contains(chatMessage.Message.Split()[1][i]))
                                            {
                                                if (game.PlacementLetters.Any(d => d != chatMessage.Message.Split()[1][i]))
                                                {
                                                    game.PlacementLetters[i] = chatMessage.Message.Split()[1][i];
                                                }
                                                result += $"{Emoji.OrangeSquare} {char.ToUpper(chatMessage.Message.Split()[1][i]).ZeroWidth()}";
                                            }
                                            else
                                            {
                                                if (game.FalseLetters.Any(d => d != chatMessage.Message.Split()[1][i]))
                                                {
                                                    game.FalseLetters.Add(chatMessage.Message.Split()[1][i]);
                                                }
                                                result += Emoji.BlackLargeSquare;
                                            }
                                        }
                                        result = $"APU @{chatMessage.Username}, {result} || That was you last try unfortunately you didn't guessed it :/ the word was: {TwitchBot.WordleGames.FirstOrDefault(d => d.Username == chatMessage.Username).Word}";
                                        TwitchBot.WordleGames.Remove(TwitchBot.WordleGames.FirstOrDefault(d => d.Username == chatMessage.Username));
                                        WordleTimer saveTimer = new(chatMessage.Channel, chatMessage.Username, twitchBot);
                                        WordleTimer.Timers.Add(saveTimer);
                                        return result;
                                    }
                                    else
                                    {
                                        string result = $"/me APU @{chatMessage.Username}, congratulations you've solved it! VisLaud You earned 50 points.";
                                        TwitchBot.WordleGames.Remove(TwitchBot.WordleGames.FirstOrDefault(d => d.Username == chatMessage.Username));
                                        WordleTimer saveTimer = new(chatMessage.Channel, chatMessage.Username, twitchBot);
                                        WordleTimer.Timers.Add(saveTimer);
                                        return result;
                                    }
                                }
                            }
                            else
                            {
                                return $"/me APU @{chatMessage.Username}, the word has to be 5 letters!";
                            }
                        }
                        else
                        {
                            return string.Empty;
                        }
                    }
                    else
                    {
                        return $"/me APU @{chatMessage.Username}, you're currently not in a game!";
                    }
                }
            }
            else
            {
                return $"/me APU {StringHelper.Rank(chatMessage.Username)}  @{chatMessage.Username}, you can solve your next wordle in {TimeHelper.ConvertUnixTimeToTimeStamp(TimeHelper.Now() - (long)WordleTimer.Timers.FirstOrDefault(d => d.Username == chatMessage.Username).SaveTimer.RemainingTime)}";
            }
        }
    }
}
