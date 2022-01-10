using ApuDoingStuff.Database.Models;
using System.Linq;

namespace ApuDoingStuff.Database
{
    public class DbController
    {
        public static void AddEmoteNr(string username, string emoteNr)
        {
            BotdbContext database = new();
            database.Dicegamedbs.FirstOrDefault(d => d.UserName == username).EmoteNr += emoteNr;
            database.SaveChanges();
        }

        public static void AddAcsNr(string username, string acsNr)
        {
            BotdbContext database = new();
            database.Dicegamedbs.FirstOrDefault(d => d.UserName == username).AcsNr += acsNr;
            database.SaveChanges();
        }

        public static void AddPoints(string username, int points)
        {
            BotdbContext database = new();
            database.Dicegamedbs.FirstOrDefault(d => d.UserName == username).Points += points;
            database.SaveChanges();
        }

        public static Channel GetChannels(string split1)
        {
            BotdbContext database = new();
            return database.Channels.FirstOrDefault(d => d.Channel1 == split1);
        }

        public static Dicegamedb GetFirstOrDefault(string username)
        {
            BotdbContext database = new();
            return database.Dicegamedbs.FirstOrDefault(d => d.UserName == username);
        }
        public static string GetRank(string username)
        {
            BotdbContext database = new();
            return database.Dicegamedbs.FirstOrDefault(d => d.UserName == username).Rank;
        }

        public static string GetAcs(string username)
        {
            BotdbContext database = new();
            return database.Dicegamedbs.FirstOrDefault(d => d.UserName == username).Accessoire;
        }

        public static void SetRank(string username, string emote)
        {
            BotdbContext database = new();
            database.Dicegamedbs.FirstOrDefault(d => d.UserName == username).Rank = emote;
            database.SaveChanges();
        }

        public static void SetAcs(string username, string acs)
        {
            BotdbContext database = new();
            database.Dicegamedbs.FirstOrDefault(d => d.UserName == username).Accessoire = acs;
            database.SaveChanges();
        }

        public static void SubPoints(string username, int points)
        {
            BotdbContext database = new();
            database.Dicegamedbs.FirstOrDefault(d => d.UserName == username).Points -= points;
            database.SaveChanges();
        }
    }
}
