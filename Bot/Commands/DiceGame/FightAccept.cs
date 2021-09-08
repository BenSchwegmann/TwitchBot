namespace ApuDoingStuff.Commands.DiceGame
{
    public class FightAccept
    {
        public bool Accepted { get; set; }
        public string Opponent { get; set; }


        public FightAccept(bool accepted)
        {
            Accepted = accepted;
        }
    }
}
