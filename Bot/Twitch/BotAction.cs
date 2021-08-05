using HLE.Time;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApuDoingStuff.Commands;

namespace ApuDoingStuff.Twitch
{
    class BotAction
    {
        public static bool IsOnCooldown(string username, CommandType type)
        {
            return TwitchBot.Cooldowns.Any(c => c.Username == username && c.Type == type && c.Time > TimeHelper.Now());
        }

        public static void AddCooldown(string username, CommandType type)
        {
            if (TwitchBot.Cooldowns.Any(c => c.Username == username && c.Type == type))
            {
                TwitchBot.Cooldowns.Remove(
                    TwitchBot.Cooldowns.Where(c => c.Username == username && c.Type == type).FirstOrDefault()
                    );
                AddUserToCooldownDictionary(username, type);
            }
        }

        public static void AddUserToCooldownDictionary(string username, CommandType type)
        {
            if (!TwitchBot.Cooldowns.Any(c => c.Username == username && c.Type == type))
            {
                TwitchBot.Cooldowns.Add(new Cooldown(username, type));
            }
        }

        public static bool IsOnMessageCooldown(string username, MessageType type)
        {
            return TwitchBot.MessageCooldowns.Any(c => c.Username == username && c.Type == type && c.Time > TimeHelper.Now());
        }

        public static void AddMessageCooldown(string username, MessageType type)
        {
            if (TwitchBot.MessageCooldowns.Any(c => c.Username == username && c.Type == type))
            {
                TwitchBot.MessageCooldowns.Remove(
                    TwitchBot.MessageCooldowns.Where(c => c.Username == username && c.Type == type).FirstOrDefault()
                    );
                AddUserToMessageCooldownDictionary(username, type);
            }
        }

        public static void AddUserToMessageCooldownDictionary(string username, MessageType type)
        {
            if (!TwitchBot.MessageCooldowns.Any(c => c.Username == username && c.Type == type))
            {
                TwitchBot.MessageCooldowns.Add(new(username, type));
            }
        }


    }
}
