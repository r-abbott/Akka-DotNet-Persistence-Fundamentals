using Akka.Actor;
using Akka.Persistence;
using PS_Akka_DotNet.Commands;
using PS_Akka_DotNet.Events;

namespace PS_Akka_DotNet.ActorModel
{
    internal class PlayerCoordinatorActor : ReceivePersistentActor
    {
        private const int DefaultStartingHealth = 100;

        public override string PersistenceId { get; } = "PlayerCoordinator";

        public PlayerCoordinatorActor()
        {
            Command<CreatePlayer>(command =>
            {
                DisplayHelper.WriteInfo($"PlayerCoordinatorActor received CreatePlayerMessage for {command.PlayerName}");

                var @event = new PlayerCreated(command.PlayerName);

                DisplayHelper.WriteInfo($"PlayerCoordinatorActor persisting PlayerCreated event for {command.PlayerName}");

                Persist(@event, playerCreatedEvent =>
                {
                    DisplayHelper.WriteInfo($"PlayerCoordinatorActor persisted PlayerCreated event for {command.PlayerName}");

                    Context.ActorOf(Props.Create(() =>
                        new PlayerActor(playerCreatedEvent.PlayerName, DefaultStartingHealth)), playerCreatedEvent.PlayerName);
                });
            });

            Recover<PlayerCreated>(playerCreatedEvent =>
            {
                DisplayHelper.WriteInfo($"PlayerCoordinatorActor replaying PlayerCreated event for {playerCreatedEvent.PlayerName}");

                Context.ActorOf(Props.Create(() =>
                        new PlayerActor(playerCreatedEvent.PlayerName, DefaultStartingHealth)), playerCreatedEvent.PlayerName);
            });
        }

    }
}
