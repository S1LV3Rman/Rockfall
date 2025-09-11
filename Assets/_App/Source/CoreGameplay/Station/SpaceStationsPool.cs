namespace S1LV3Rman.RockFall.CoreGameplay
{
    public sealed class SpaceStationsPool : AliveObjectsPoolWithRegistry<SpaceStation>
    {
        public SpaceStationsPool(InstanceRegistry<IDamageableProvider> damageables) 
            : base(damageables)
        {
        }
    }
}