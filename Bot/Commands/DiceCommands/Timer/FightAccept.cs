namespace ApuDoingStuff.Commands.DiceGame
{
#pragma warning disable CS0659
    public class FightAccept
    {
        public bool Accepted { get; set; } = false;
        public string Opponent { get; set; }
        public string Channel { get; set; }
        public int Points { get; set; }
        public string Challenger { get; set; }

        public FightAccept(string opponent, string channel, int points, string challenger)
        {
            Opponent = opponent;
            Channel = channel;
            Points = points;
            Challenger = challenger;
        }

        public override bool Equals(object obj)
        {
            return obj is FightAccept fight && fight.Challenger == Challenger && fight.Opponent == Opponent && fight.Points == Points && fight.Channel == Channel;
        }
    }

}
