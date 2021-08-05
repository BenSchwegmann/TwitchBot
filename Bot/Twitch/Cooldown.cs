using ApuDoingStuff.Commands;
using HLE.Time;

namespace ApuDoingStuff.Twitch
{
    public class Cooldown
    {
        public string Username { get; set; }

        public CommandType Type { get; set; }

        public long Time { get; private set; }


        public Cooldown(string username, CommandType type)
        {
            Username = username;
            Type = type;
            Time = TimeHelper.Now() + CommandHelper.GetCommand(type).Cooldown;
        }
    }
}