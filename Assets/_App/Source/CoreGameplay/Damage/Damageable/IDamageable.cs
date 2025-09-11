using R3;

namespace S1LV3Rman.RockFall.CoreGameplay
{
    public interface IDamageable
    {
        string Name { get; }
        public DamageModifier Modifier { get; }
        public ReadOnlyReactiveProperty<int> CurrentHealth { get; }
        public int MaxHealth { get; }
        public void SetMax(int maxHealth, CurrentHealthChange currentDependence = CurrentHealthChange.SamePercent);
        public void ReceiveDamage(DamageContext context);
        public void ApplyDamage(DamageContext context, int damage);
        public Observable<DamageContext> OnReceivingDamage { get; }
        public Observable<DamageEvent> OnDamaged { get; }
    }
}