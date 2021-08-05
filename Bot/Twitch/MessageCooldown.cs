using HLE.Time;
using ApuDoingStuff.Messages;
using ApuDoingStuff.Commands;
using ApuDoingStuff.Commands.CommandClasses;

namespace ApuDoingStuff.Twitch
{
    public class MessageCooldown
    {
        public string Username { get; set; }

        public MessageType Type { get; set; }

        public long Time { get; private set; }


        public MessageCooldown(string username, MessageType type)
        {
            Username = username;
            Type = type;
            Time = TimeHelper.Now() + CommandHelper.GetMessageCommand(type).Cooldown;
        }
    }
}