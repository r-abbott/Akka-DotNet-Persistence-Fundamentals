using Akka.Actor;
using PS_Akka_DotNet.ActorModel;
using PS_Akka_DotNet.Commands;
using System;
using System.Threading;

namespace PS_Akka_DotNet
{
    class Program
    {
        private static ActorSystem System { get; set; }
        private static IActorRef PlayerCoordinator { get; set; }

        static void Main(string[] args)
        {
            System = ActorSystem.Create("Game");
            PlayerCoordinator = System.ActorOf<PlayerCoordinatorActor>("PlayerCoordinator");

            DisplayInstructions();

            while (true)
            {
                var action = Console.ReadLine();

                var playerName = action.Split(' ')[0];

                if (action.Contains("create"))
                {
                    CreatePlayer(playerName);
                }
                else if (action.Contains("hit"))
                {
                    var damage = int.Parse(action.Split(' ')[2]);
                    HitPlayer(playerName, damage);
                }
                else if (action.Contains("display"))
                {
                    DisplayPlayer(playerName);
                }
                else if (action.Contains("error"))
                {
                    ErrorPlayer(playerName);
                }
                else
                {
                    DisplayHelper.WriteResult("Unknown command");
                }
            }
        }

        private static void CreatePlayer(string playerName)
        {
            PlayerCoordinator.Tell(new CreatePlayer(playerName));
        }

        private static void DisplayPlayer(string playerName)
        {
            System.ActorSelection($"/user/PlayerCoordinator/{playerName}")
                .Tell(new DisplayStatus());
        }

        private static void ErrorPlayer(string playerName)
        {
            System.ActorSelection($"/user/PlayerCoordinator/{playerName}")
                .Tell(new SimulateError());
        }

        private static void HitPlayer(string playerName, int damage)
        {
            System.ActorSelection($"/user/PlayerCoordinator/{playerName}")
                .Tell(new HitPlayer(damage));
        }

        private static void DisplayInstructions()
        {
            Thread.Sleep(500);

            DisplayHelper.WriteLine("Available commands:");
            DisplayHelper.WriteLine("<playername> create");
            DisplayHelper.WriteLine("<playername> display");
            DisplayHelper.WriteLine("<playername> error");
            DisplayHelper.WriteLine("<playername> hit");
        }
    }
}
