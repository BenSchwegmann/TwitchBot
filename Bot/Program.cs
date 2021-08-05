using ApuDoingStuff.Jsons;
using ApuDoingStuff.Twitch;
using System;


namespace ApuDoingStuff
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            JsonController.LoadData();
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

