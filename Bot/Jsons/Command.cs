using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApuDoingStuff.Jsons
{
    public class Command
    {
        public string CommandName { get; set; }
        public string Descripton { get; set; }
        public int NeededPermisson { get; set; }
        public int Cooldown { get; set; }

    }
}
