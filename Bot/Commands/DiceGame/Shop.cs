using ApuDoingStuff.Jsons;
using ApuDoingStuff.Twitch;
using TwitchLib.Client.Models;

namespace ApuDoingStuff.Commands.CommandClasses
{
    public class Shop
    {
        public static void Handle(TwitchBot twitchBot, ChatMessage chatMessage)
        {
            string result = "";
            JsonController.Ranks.ForEach(r => result += $"{r.Name} No.: {r.EmoteNr}, Price: {r.Price} || ");
            twitchBot.Send(chatMessage.Channel, $"/me APU @{chatMessage.Username}, here is a list of all emotes you can buy and their prices: {result}");
        }
    }
}
