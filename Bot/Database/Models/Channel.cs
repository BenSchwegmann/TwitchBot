#nullable disable

namespace ApuDoingStuff.Database.Models
{
    public partial class Channel
    {
        public int Id { get; set; }
        public string Channel1 { get; set; }
        public bool? IfLive { get; set; } = false;
    }
}
