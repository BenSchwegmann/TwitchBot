using System.Collections.Generic;

namespace ApuDoingStuff.Jsons
{
    public class CommandList
    {
        public List<Command> Commands { get; set; }
        public List<MessageCommandsCooldown> MessageCommands { get; set; }

    }
}
