using R3;

namespace S1LV3Rman.RockFall
{
    public interface IAliveTrackedObject
    {
        public ReadOnlyReactiveProperty<bool> IsAlive { get; }
    }
}