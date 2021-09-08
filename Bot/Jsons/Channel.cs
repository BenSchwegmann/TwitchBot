namespace ApuDoingStuff.Jsons
{
    public class Channel
    {

        public string Name { get; set; }
        public bool State { get; set; }

        public Channel(string name, bool state)
        {
            Name = name;
            State = state;
        }
    }
}
