using ApuDoingStuff.Database.Models;
using System.Linq;
using TwitchLib.Client.Models;

namespace ApuDoingStuff.Database
{
    public class DbController
    {
        private static readonly BotdbContext _database = new();
        public static Dicegamedb GetFirstUser(ChatMessage chatMessage)
        {
            return _database.Dicegamedbs.FirstOrDefault(d => d.UserName == chatMessage.Username);
        }

        public static Dicegamedb GetFirstEnemy(string enemy)
        {
            return _database.Dicegamedbs.FirstOrDefault(d => d.UserName == enemy);
        }

        public static Dicegamedb GetFirstChallenger(string challenger)
        {
            return _database.Dicegamedbs.FirstOrDefault(d => d.UserName == challenger);
        }

        public static Dicegamedb GetFirstWinner(string winner)
        {
            return _database.Dicegamedbs.FirstOrDefault(d => d.UserName == winner);
        }

        public static Dicegamedb GetFirstSplit1(ChatMessage chatMessage)
        {
            return _database.Dicegamedbs.FirstOrDefault(d => d.UserName == chatMessage.Message.Split()[1]);
        }
    }
}
