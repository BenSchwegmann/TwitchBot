using ApuDoingStuff.Commands;
using ApuDoingStuff.Twitch;
using TwitchLib.Client.Models;

namespace ApuDoingStuff.Messages
{
    public static class MessageCommands
    {
        public static void ApuSquats(TwitchBot twitchBot, ChatMessage chatMessage)
        {

            if (chatMessage.Message == "ApuSquats" && !BotAction.IsOnMessageCooldown(chatMessage.Username, MessageType.ApuSquats))
            {
                BotAction.AddUserToMessageCooldownDictionary(chatMessage.Username, MessageType.ApuSquats);
                twitchBot.Send(chatMessage.Channel, "ApuSquats");
                BotAction.AddMessageCooldown(chatMessage.Username, MessageType.ApuSquats);
            }
        }

    }
}
