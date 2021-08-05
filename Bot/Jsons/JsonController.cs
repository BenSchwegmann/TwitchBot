using ApuDoingStuff.Properties;
using System.IO;
using System.Text.Json;

namespace ApuDoingStuff.Jsons
{
    public class JsonController
    {
        public static CommandList CommandList { get; private set; }
        public static void LoadData()
        {
            CommandList = JsonSerializer.Deserialize<CommandList>(File.ReadAllText(Resources.CommandsJsonPath));
        }

    }
}
