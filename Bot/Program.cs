using System;
using ApuDoingStuff;
using ApuDoingStuff.Twitch;


namespace ApuDoingStuff
{
    class Program
    {
        static void Main(string[] args)
        {
            new TwitchBot().SetBot();
            Console.ReadLine();
        }
        public static void ConsoleOut(string value, ConsoleColor fontColor = ConsoleColor.Gray)
        {
            Console.ForegroundColor = fontColor;
            Console.WriteLine($"{DateTime.Now:HH:mm:ss} | {value}");
            Console.ForegroundColor = ConsoleColor.Gray;
            
        }
    }

}
      
