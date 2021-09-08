using ApuDoingStuff.Commands;
using ApuDoingStuff.Twitch;
using HLE.Strings;
using TwitchLib.Client.Models;

namespace ApuDoingStuff.Messages
{
    public static class MessageHelper
    {
        public static string CommandCleaner(TwitchBot twitchBot, ChatMessage chatMessage, CommandType type)
        {
            return chatMessage.Message.Split()[1..].ToSequence();
        }
    }
}
