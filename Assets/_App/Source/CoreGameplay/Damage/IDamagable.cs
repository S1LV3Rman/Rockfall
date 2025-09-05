using R3;

namespace S1LV3Rman.RockFall.CoreGameplay
{
    public interface IDamageable
    {
        bool CanReceive(DamageContext ctx);
        void Receive(DamageContext ctx);
        ReadOnlyReactiveProperty<int> Current { get; }
        int Max { get; }
        Observable<DamageEvent> OnDamaged { get; }
    }
}