using System;
using System.Collections.Generic;

#nullable disable

namespace ApuDoingStuff.Database.Models
{
    public partial class Banuserconfig
    {
        public int Id { get; set; }
        public string ChannelName { get; set; }
        public bool? State { get; set; }
    }
}
