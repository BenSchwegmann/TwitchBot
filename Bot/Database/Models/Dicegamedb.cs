#nullable disable

namespace ApuDoingStuff.Database.Models
{
    public partial class Dicegamedb
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public int? Points { get; set; } = 0;
        public bool? PingMe { get; set; }
    }
}
