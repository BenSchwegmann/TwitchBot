using System.Collections.Generic;
using System.Linq;

namespace ApuDoingStuff.Jsons
{
    public class Accessoires
    {
        public int AcsNr { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public bool IsZeroWidth { get; set; }

        public static Accessoires GetAccessoire(int acsNr)
        {
            return JsonController.Accessoires.FirstOrDefault(a => a.AcsNr == acsNr);
        }

        public static Accessoires GetAccessoire(string acs)
        {
            return JsonController.Accessoires.FirstOrDefault(a => a.Name == acs);
        }

        public static Accessoires[] GetAccessoire(string[] ids)
        {
            List<string> intersectAcs = JsonController.Accessoires.Select(r => r.AcsNr.ToString()).Intersect(ids).ToList();
            return intersectAcs.Select(i => JsonController.Accessoires.FirstOrDefault(r => r.AcsNr.ToString() == i)).ToArray();
        }
    }
}
