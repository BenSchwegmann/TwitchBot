using ApuDoingStuff.Jsons;
using ApuDoingStuff.Twitch;
using System;
using System.Diagnostics;
using System.Threading;

namespace ApuDoingStuff
{
    internal class Program
    {
        private static bool _running = true;

        private static void Main(string[] args)
        {
            JsonController.LoadData();
            TwitchApi.Configure();
            new TwitchBot().SetBot();

            while (_running)
            {
                Thread.Sleep(1000);
            }
        }
        public static void ConsoleOut(string value, ConsoleColor fontColor = ConsoleColor.Gray)
        {
            Console.ForegroundColor = fontColor;
            Console.WriteLine($"{DateTime.Now:HH:mm:ss} | {value}");
            Console.ForegroundColor = ConsoleColor.Gray;

        }
        public static void Restart()
        {
            ConsoleOut($"BOT>RESTARTED", ConsoleColor.Red);
            Process.Start($"./ApuDoingStuff");
            _running = false;
            Environment.Exit(0);
        }
    }

}

