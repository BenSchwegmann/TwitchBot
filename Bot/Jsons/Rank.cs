using System.Collections.Generic;
using System.Linq;

namespace ApuDoingStuff.Jsons
{
    public class Rank
    {
        public int EmoteNr { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }

        public static Rank GetRank(int emoteNr)
        {
            return JsonController.Ranks.FirstOrDefault(a => a.EmoteNr == emoteNr);
        }

        public static Rank GetRank(string name)
        {
            return JsonController.Ranks.FirstOrDefault(a => a.Name == name);
        }

        public static Rank[] GetRank(string[] ids)
        {
            List<string> intersectRanks = JsonController.Ranks.Select(r => r.EmoteNr.ToString()).Intersect(ids).ToList();
            return intersectRanks.Select(i => JsonController.Ranks.FirstOrDefault(r => r.EmoteNr.ToString() == i)).ToArray();
        }
    }
}
