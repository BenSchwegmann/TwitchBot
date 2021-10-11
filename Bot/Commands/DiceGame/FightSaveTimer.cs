using ApuDoingStuff.Twitch;
using System.Linq;
using System.Timers;

namespace ApuDoingStuff.Commands.DiceGame
{
    public class FightSaveTimer
    {
        public Timer FightTimer { get; }
        public TwitchBot TwitchBot { get; }
        public FightSaveTimer SaveFight { get; }
        public string Username { get; set; }
        public string Channel { get; set; }
        public string Challenger { get; set; }

        private const int _timer = 60000;

        public FightSaveTimer(string channel, string username, string challenger, TwitchBot twitchBot)
        {
            Channel = channel;
            Challenger = challenger;
            Username = username;
            TwitchBot = twitchBot;
            FightTimer = new(_timer);
            FightTimer.Elapsed += OnTimedEvent;
            FightTimer.AutoReset = false;
            FightTimer.Start();
        }

        public void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            if (TwitchBot.FightAccepts.Any(d => d.Opponent == Username))
            {
                FightAccept fightAccept = TwitchBot.FightAccepts.FirstOrDefault(d => d.Opponent == Username);
                TwitchBot.FightTimerExpired(TwitchBot, fightAccept.Channel, fightAccept.Opponent, fightAccept.Challenger);
                TwitchBot.FightAccepts.Remove(TwitchBot.FightAccepts.FirstOrDefault(d => d.Opponent == Username));
            }
        }
    }

}
