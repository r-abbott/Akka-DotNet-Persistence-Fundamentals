namespace PS_Akka_DotNet.Events
{
    class PlayerHit
    {
        public int Damage { get; }

        public PlayerHit(int damage)
        {
            Damage = damage;
        }
    }
}
