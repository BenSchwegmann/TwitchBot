using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApuDoingStuff.Properties;

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
