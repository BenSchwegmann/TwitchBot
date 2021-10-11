using ApuDoingStuff.Database.Models;
using ApuDoingStuff.Jsons;
using ApuDoingStuff.Twitch;
using HLE.Emojis;
using HLE.Strings;
using System.Linq;
using TwitchLib.Client.Models;

namespace ApuDoingStuff.Commands.CommandClasses
{
    public class Buy
    {
        public static void Handle(TwitchBot twitchBot, ChatMessage chatMessage)
        {
            if (chatMessage.Message.Split().Length >= 2)
            {
                BotdbContext database = new();
                int emoteNr = chatMessage.Message.Split()[1].ToInt();
                if (chatMessage.Message.Split()[1].IsMatch(@"^\d+$") && JsonController.Ranks.Any(r => r.EmoteNr == emoteNr))
                {
                    JsonController.Ranks.ForEach(r =>
                    {
                        if (r.EmoteNr == emoteNr)
                        {
                            if (database.Dicegamedbs.FirstOrDefault(d => d.UserName == chatMessage.Username).Points >= r.Price)
                            {
                                _ = database.Dicegamedbs.FirstOrDefault(d => d.UserName == chatMessage.Username).Rank = r.Name;
                                database.Dicegamedbs.FirstOrDefault(d => d.UserName == chatMessage.Username).Points -= r.Price;
                                database.SaveChanges();
                                twitchBot.Send(chatMessage.Channel, $"/me APU {Emoji.Tada} congratulations @{chatMessage.Username} you bought the emote: {r.Name}");
                            }
                            else
                            {
                                twitchBot.Send(chatMessage.Channel, $"/me APU @{chatMessage.Username}, you don't have enough points to buy this emote!");
                            }
                        }
                    });
                }
                else
                {
                    twitchBot.Send(chatMessage.Channel, $"/me APU @{chatMessage.Username}, this is not a valid emote number! Type \"?shop\" to see a list of all emotes, emote numbers and their prices.");
                }
            }
            else
            {
                twitchBot.Send(chatMessage.Channel, $"/me APU @{chatMessage.Username}, please enter the emote number! Type \"?shop\" to see a list of all emotes, emote numbers and their prices.");
            }
        }

    }
}
