using ApuDoingStuff.Twitch;
using HLE.Strings;
using System;
using System.Linq;
using TwitchLib.Client.Models;

namespace ApuDoingStuff.Commands
{
    public static class CommandHandler
    {
        public static void Handle(TwitchBot twitchBot, ChatMessage chatMessage)
        {
            if (chatMessage.Channel is not "pajlada")
            {
                ((CommandType[])Enum.GetValues(typeof(CommandType))).ToList().ForEach(type =>
                {
                    if (!BotAction.IsOnCooldown(chatMessage.Username, type))
                    {
                        if (chatMessage.Message.IsMatch(@"^\?" + type.ToString() + @"(\s|$)"))
                        {
                            BotAction.AddUserToCooldownDictionary(chatMessage.Username, type);
                            Type.GetType($"ApuDoingStuff.Commands.CommandClasses.{type}").GetMethod("Handle").Invoke(null, new object[] { twitchBot, chatMessage });
                            BotAction.AddCooldown(chatMessage.Username, type);
                        }
                    }
                });
            }
        }
    }
}
