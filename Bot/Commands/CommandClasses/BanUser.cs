using ApuDoingStuff.Database.Models;
using ApuDoingStuff.Twitch;
using System.Linq;
using TwitchLib.Client.Models;

namespace ApuDoingStuff.Commands.CommandClasses
{
    public static class BanUser
    {


        public static void Handle(TwitchBot twitchBot, ChatMessage chatMessage)
        {

            if (new BotdbContext().Banuserconfigs.FirstOrDefault(b => b.ChannelName == chatMessage.Channel).State == true)
            {
                if (chatMessage.Message.Split()[1].ToLower() == chatMessage.BotUsername || chatMessage.Message.Split()[1].ToLower() == chatMessage.Channel)
                {
                    twitchBot.Send(chatMessage.Channel, $"/me APU no, i don't think so @{chatMessage.Username}!");
                }
                else
                {
                    if (chatMessage.Message.Split().Length <= 2)
                    {
                        twitchBot.Send(chatMessage.Channel, $"/ban {chatMessage.Message.Split()[1]}");
                        twitchBot.Send(chatMessage.Channel, $"/me MODS DONE!");
                    }
                    else
                    {
                        twitchBot.Send(chatMessage.Channel, $"/me APU @{chatMessage.Username}, please enter a valid user you would like to ban!");
                    }
                }

            }
            else
            {
                twitchBot.Send(chatMessage.Channel, $"/me APU @{chatMessage.Username}, if you want to enable this command write \"?set banUser 1\"!");
            }

        }


    }

}

