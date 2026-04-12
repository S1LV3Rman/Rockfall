namespace S1LV3Rman.RockFall.CoreGameplay
{
    public sealed class AsteroidsPool : AliveObjectsPoolWithRegistry<Asteroid>
    {
        public AsteroidsPool(InstanceRegistry<IDamageableProvider> damageables) 
            : base(damageables)
        {
        }
    }
}