using ApuDoingStuff.Database;
using ApuDoingStuff.Database.Models;
using ApuDoingStuff.Twitch;
using HLE.Strings;
using System.Linq;
using TwitchLib.Client.Models;
using StringHelper = ApuDoingStuff.Utils.StringHelper;

namespace ApuDoingStuff.Commands.CommandClasses
{
    public static class Unset
    {
        public static void Handle(TwitchBot twitchBot, ChatMessage chatMessage)
        {
            if (chatMessage.Message.IsMatch(@"^\?unset\s+(acs|accessoire)"))
            {
                Dicegamedb user = DbController.GetFirstOrDefault(chatMessage.Username);
                if (user.Accessoire.Any())
                {
                    if (user.AcsNr.Any())
                    {
                        DbController.SetAcs(chatMessage.Username, null);
                        twitchBot.Send(chatMessage.Channel, $"/me APU {StringHelper.Rank(chatMessage.Username)} @{chatMessage.Username}, you have successfully unequiped your accessoire");
                    }
                    else
                    {
                        twitchBot.Send(chatMessage.Channel, $"/me {StringHelper.Rank(chatMessage.Username)} @{chatMessage.Username}, you have no accessories!");
                    }
                }
                else
                {
                    twitchBot.Send(chatMessage.Channel, $"/me APU {StringHelper.Rank(chatMessage.Username)} @{chatMessage.Username}, you have currently no accessorie equiped!");
                }
            }
        }
    }
}
