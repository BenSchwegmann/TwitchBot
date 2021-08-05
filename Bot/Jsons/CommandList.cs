using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApuDoingStuff.Jsons
{
    public class CommandList
    {
        public List<Command> Commands { get; set; }
        public List<MessageCommandsCooldown> MessageCommandsCooldowns { get; set; }

    }
}
