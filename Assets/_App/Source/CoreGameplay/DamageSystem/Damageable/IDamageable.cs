using R3;

namespace S1LV3Rman.RockFall.CoreGameplay
{
    public interface IDamageable
    {
        public IUnit Owner { get; }
        public ReadOnlyReactiveProperty<int> CurrentHealth { get; }
        public ReadOnlyReactiveProperty<int> MaxHealth { get; }
        public void SetMax(int maxHealth, CurrentHealthChange currentDependence = CurrentHealthChange.SamePercent);
        public void ApplyDamage(DamageContext context, int damage);
        public Observable<DamageEvent> OnDamaged { get; }
    }
}