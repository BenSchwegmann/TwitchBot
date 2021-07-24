using System;


namespace Bot
{
    class Program
    {
        static void Main(string[] args)
        {
            Bot bot = new Bot();

            bot.Connect(true);

            Console.ReadLine();

            bot.Disconnect();
        }

    }

}
      
