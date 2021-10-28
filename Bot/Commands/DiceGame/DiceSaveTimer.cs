using ApuDoingStuff.Twitch;
using HLE.Time;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ApuDoingStuff.Commands.DiceGame
{
    public class DiceSaveTimer
    {
        public string Channel { get; }
        public string Username { get; }
        public HTimer SaveTimer { get; }
        public TwitchBot TwitchBot { get; }
        public static List<DiceSaveTimer> Timers { get; } = new();

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

        public void OnTimedEvent(object source, EventArgs e)
        {
            Timers.Remove(Timers.FirstOrDefault(d => d.Username == Username));
            TwitchBot.SendDicePing(TwitchBot, Channel, Username);
        }
    }
}

