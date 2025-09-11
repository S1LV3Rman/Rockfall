namespace S1LV3Rman.RockFall.CoreGameplay
{
    public interface IDamageDealer
    {
        public IInstigator Source { get; }
        public DamageType DamageType { get; }
        public int BaseDamage { get; }
        public DamageModifier Modifier { get; }
    }
}