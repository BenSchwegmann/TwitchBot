using ApuDoingStuff.Properties;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace ApuDoingStuff.Jsons
{
    public static class JsonController
    {
        public static CommandList CommandList { get; private set; }
        public static List<Rank> Ranks { get; private set; }
        public static List<Accessoires> Accessoires { get; private set; }

        public static List<string> Words { get; private set; }

        public static void LoadData()
        {
            CommandList = JsonSerializer.Deserialize<CommandList>(File.ReadAllText(Resources.CommandsJsonPath));
            Ranks = JsonSerializer.Deserialize<List<Rank>>(File.ReadAllText(Resources.RanksJsonPath));
            Accessoires = JsonSerializer.Deserialize<List<Accessoires>>(File.ReadAllText(Resources.AcsJsonPath));
            Words = JsonSerializer.Deserialize<List<string>>(File.ReadAllText(Resources.WordsJsonPath));
        }

    }
}
