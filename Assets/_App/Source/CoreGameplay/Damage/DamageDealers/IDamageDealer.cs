namespace S1LV3Rman.RockFall.CoreGameplay
{
    public interface IDamageDealer
    {
        public IInstigator Source { get; }
        public int Damage { get; }
        public DamageType Type { get; }
        public int TeamId { get; }
    }
}