using HLE.Strings;
using System.Collections.Generic;

namespace ApuDoingStuff.Twitch
{
    public class DividedMessages
    {
        public TwitchBot TwitchBot { get; }
        public string Channel { get; }
        public List<string> Messages { get; }

        public DividedMessages(TwitchBot twitchBot, string channel, string messages)
        {
            TwitchBot = twitchBot;
            Channel = channel;
            Messages = messages.Split(TwitchBot.MaxLenght);
        }
    }
}
