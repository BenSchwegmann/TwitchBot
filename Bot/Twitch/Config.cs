using ApuDoingStuff.Database.Models;
using System.Collections.Generic;
using System.Linq;

namespace ApuDoingStuff.Twitch
{
    public static class Config
    {
        public static List<string> GetChannels()
        {
            BotdbContext database = new();
            List<string> channelnames = database.Channels.Select(d => d.Channel1).ToList();
            return channelnames;
        }

    }
}
