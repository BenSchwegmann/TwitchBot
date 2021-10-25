using ApuDoingStuff.Twitch;
using System.Timers;

namespace ApuDoingStuff.Commands.CommandClasses
{
    public class BigDiceSaveTimer
    {
        public string Channel { get; }
        public string Username { get; }
        public Timer SaveTimer { get; }
        public TwitchBot TwitchBot { get; }


        private const int _timer = 10800000;

        public BigDiceSaveTimer(string channel, string username, TwitchBot twitchBot)
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
