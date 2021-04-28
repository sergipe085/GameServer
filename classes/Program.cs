using System;

namespace GameServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Game Server";
            
            Server.Start(4, 3333);

            Console.ReadKey();
        }
    }
}
