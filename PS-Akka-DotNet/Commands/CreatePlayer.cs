namespace PS_Akka_DotNet.Commands
{
    internal class CreatePlayer
    {
        public string PlayerName { get; }

        public CreatePlayer(string playerName)
        {
            PlayerName = playerName;
        }
    }
}
