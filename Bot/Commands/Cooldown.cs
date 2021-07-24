using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HLE.Time;

namespace Bot
{
    public class Cooldown
    {

        public string Username { get; set; }

        public CommandType Type { get; set; }

        public long Time { get; private set; }

        public Cooldown(string username, CommandType type)
        {
            Username = username;
            Type = type;
            Time = TimeHelper.Now();
        }
    }
}
