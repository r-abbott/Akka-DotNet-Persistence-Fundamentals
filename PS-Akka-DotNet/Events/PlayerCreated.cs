namespace PS_Akka_DotNet.Events
{
    class PlayerCreated
    {
        public string PlayerName { get; }

        public PlayerCreated(string playerName)
        {
            PlayerName = playerName;
        }
    }
}
