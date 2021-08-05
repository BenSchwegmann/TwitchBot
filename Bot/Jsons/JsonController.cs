using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApuDoingStuff.Properties;
using System.Text.Json;
using System.IO;

namespace ApuDoingStuff.Jsons
{
    public class JsonController
    {
        public static CommandList CommandList { get; private set; }
        public static void LoadData ()
        {
            CommandList = JsonSerializer.Deserialize<CommandList>(File.ReadAllText(Resources.CommandsJsonPath));
        }

    }
}
