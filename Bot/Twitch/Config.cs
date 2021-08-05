using ApuDoingStuff.Properties;
using System.Collections.Generic;
using System.Linq;

namespace ApuDoingStuff.Twitch
{
    public static class Config
    {
        public static List<string> GetChannels()
        {
            return Resources.TwitchChannel.Split().ToList();
        }

    }
}
