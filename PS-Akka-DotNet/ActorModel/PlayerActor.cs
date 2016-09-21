using System;
using Akka.Persistence;
using PS_Akka_DotNet.Commands;
using PS_Akka_DotNet.Events;

namespace PS_Akka_DotNet.ActorModel
{
    internal class PlayerActorState
    {
        public string PlayerName { get; set; }
        public int Health { get; set; }

        public override string ToString()
        {
            return $"[PlayerActorState {PlayerName} {Health}]";
        }
    }

    internal class PlayerActor : ReceivePersistentActor
    {
        private PlayerActorState _state;
        private int _eventCount;


        public override string PersistenceId => $"player-{_state.PlayerName}";

        public PlayerActor(string playerName, int startingHealth)
        {
            _state = new PlayerActorState
            {
                PlayerName = playerName,
                Health = startingHealth
            };

            DisplayHelper.WriteResult($"{_state.PlayerName} created");
            
            Command<HitPlayer>(x => HitPlayer(x));
            Command<DisplayStatus>(x => DisplayPlayerStatus());
            Command<SimulateError>(x => SimulateError());

            Recover<PlayerHit>(@event =>
            {
                DisplayHelper.WriteInfo($"{_state.PlayerName} replaying PlayerHit event from journal");
                _state.Health -= @event.Damage;
            });

            Recover<SnapshotOffer>(offer =>
            {
                DisplayHelper.WriteInfo($"{_state.PlayerName} received SnapshotOffer from snapshot store, updating state");

                _state = (PlayerActorState)offer.Snapshot;

                DisplayHelper.WriteInfo($"{_state.PlayerName} state {_state} set from snapshot");
            });
        }

        private void HitPlayer(HitPlayer command)
        {
            DisplayHelper.WriteInfo($"{_state.PlayerName} recieved HitCommand");

            var @event = new PlayerHit(command.Damage);

            DisplayHelper.WriteInfo($"{_state.PlayerName} persisting PlayerHit event");

            Persist(@event, hitEvent =>
            {
                DisplayHelper.WriteInfo($"{_state.PlayerName} persisted PlayerHit event, updating actor state");

                _state.Health -= hitEvent.Damage;

                _eventCount++;

                if(_eventCount > 5)
                {
                    DisplayHelper.WriteInfo($"{_state.PlayerName} saving snapshot");

                    SaveSnapshot(_state);

                    _eventCount = 0;
                }
            });
        }

        private void DisplayPlayerStatus()
        {
            DisplayHelper.WriteInfo($"{_state.PlayerName} recieved DisplayStatusCommand");

            DisplayHelper.WriteResult($"{_state.PlayerName} has {_state.Health} health");
        }

        private void SimulateError()
        {
            DisplayHelper.WriteInfo($"{_state.PlayerName} recieved SimulateErrorCommand");

            throw new ApplicationException($"Simulated exception in player: {_state.PlayerName}");
        }
    }
}
