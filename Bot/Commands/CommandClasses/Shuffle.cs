using ApuDoingStuff.Twitch;
using HLE.Collections;
using System.Collections.Generic;
using System.Linq;
using TwitchLib.Client.Models;

namespace ApuDoingStuff.Commands.CommandClasses
{
    public class Shuffle
    {
        public static void Handle(TwitchBot twitchBot, ChatMessage chatMessage)
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
            twitchBot.Send(chatMessage.Channel, $"/me {result}");
        }
    }
}
