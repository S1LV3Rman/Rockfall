namespace S1LV3Rman.RockFall.CoreGameplay
{
    public interface IWeapon : IDamageDealer
    {
        public DamageType DamageType { get; }
    }
}