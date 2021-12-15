using ApuDoingStuff.API;
using ApuDoingStuff.Commands;
using ApuDoingStuff.Database.Models;
using ApuDoingStuff.Properties;
using HLE.Collections;
using HLE.Emojis;
using HLE.Time;
using System.Collections.Generic;
using System.Linq;
using TwitchLib.Client.Models;

namespace ApuDoingStuff.Twitch
{
    public class BotAction
    {
        private static readonly BotdbContext _database = new();

        public static void AddCooldown(string username, CommandType type)
        {
            if (TwitchBot.Cooldowns.Any(c => c.Username == username && c.Type == type))
            {
                TwitchBot.Cooldowns.Remove(
                    TwitchBot.Cooldowns.Where(c => c.Username == username && c.Type == type).FirstOrDefault()
                    );
                AddUserToCooldownDictionary(username, type);
            }
        }

        public static void AddMessageCooldown(string username, MessageType type)
        {
            if (TwitchBot.MessageCooldowns.Any(c => c.Username == username && c.Type == type))
            {
                TwitchBot.MessageCooldowns.Remove(
                    TwitchBot.MessageCooldowns.Where(c => c.Username == username && c.Type == type).FirstOrDefault()
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
                return $"/me APU @{chatMessage.Username}, you need to add an suggestion to your message.";
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
            return $"/me APU @{chatMessage.Username}, {HTTPRequest.RandomApuTitleUrl()} {HTTPRequest.RandomApuPicUrl()}";
        }

        public static string SendAxolotl(ChatMessage chatMessage)
        {
            return $"/me APU @{chatMessage.Username}, {HTTPRequest.RandomAxolotFact()} {HTTPRequest.RandomAxolotlUrl()}";
        }

        public static string SendCat(ChatMessage chatMessage)
        {
            return $"/me APU @{chatMessage.Username}, {HTTPRequest.RandomCatFact()} {HTTPRequest.RandomCatUrl()} CoolCat";
        }

        public static string SendDog(ChatMessage chatMessage)
        {
            return $"/me APU @{chatMessage.Username}, {HTTPRequest.RandomDogUrl()} Wowee";
        }

        public static string SendDuck(ChatMessage chatMessage)
        {
            return $"/me APU @{chatMessage.Username}, {HTTPRequest.RandomDuckUrl()} DuckerZ";
        }

        public static string SendJoin(ChatMessage chatMessage, TwitchBot twitchBot)
        {
            if (chatMessage.Message.Split().Length == 2)
            {
                string channel = chatMessage.Message.Split()[1];

                if (chatMessage.Username == Resources.Owner)
                {
                    _ = _database.Channels.Add(new Channel { Channel1 = $"{channel}" });
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
            string result = HTTPRequest.LyricsUrl(title);
            return $"/me APU @{chatMessage.Username}, {result}";
        }

        public static string SendPing(ChatMessage chatMessage, TwitchBot twitchBot)
        {
            return $"/me APU {Emoji.MagicWand} PONG! {twitchBot.GetSystemInfo()}";
        }
        public static string SendShiba(ChatMessage chatMessage)
        {
            return $"/me APU @{chatMessage.Username}, {HTTPRequest.RandomShibaUrl()} ConcernDoge";
        }

        public static string SendShuffle(ChatMessage chatMessage)
        {
            string result = string.Empty;
            List<string> words = chatMessage.Message.Split(" ")[1..].ToList();
            int length = words.Count;
            for (int i = 0; i < length; i++)
            {
                string word = words.Random();
                result += $"{word} ";
                words.Remove(word);
            }
            return $"/me {result}";
        }
    }
}
