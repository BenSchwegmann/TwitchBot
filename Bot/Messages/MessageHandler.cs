using ApuDoingStuff.Commands;
using ApuDoingStuff.Twitch;
using TwitchLib.Client.Models;

namespace ApuDoingStuff.Messages
{
    public static class MessageHandler
    {
        public static void Handle(TwitchBot twitchBot, ChatMessage chatMessage)
        {
            CommandHandler.Handle(twitchBot, chatMessage);
            MessageCommands.ApuSquats(twitchBot, chatMessage);
            MessageCommands.Alert(twitchBot, chatMessage);
        }
    }
}
