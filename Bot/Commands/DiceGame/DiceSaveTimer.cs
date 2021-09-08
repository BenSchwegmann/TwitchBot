using ApuDoingStuff.Twitch;
using System.Timers;

namespace ApuDoingStuff.Commands.DiceGame
{
    public class DiceSaveTimer
    {
        public string Channel { get; }
        public string Username { get; }
        public Timer SaveTimer { get; }
        public TwitchBot TwitchBot { get; }


        private const int _timer = 3600000;

        public DiceSaveTimer(string channel, string username, TwitchBot twitchBot)
        {
            Channel = channel;
            Username = username;
            SaveTimer = new(_timer);
            TwitchBot = twitchBot;
            SaveTimer.Elapsed += OnTimedEvent;
            SaveTimer.AutoReset = false;
            SaveTimer.Start();

        }

        public void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            TwitchBot.SendDicePing(TwitchBot, Channel, Username);
        }
    }
}

