using System;
using R3;

namespace S1LV3Rman.RockFall.CoreGameplay
{
    public interface IDamageDealer
    {
        public Guid Id { get; }
        public IDamageSource Source { get; set; }
        public DamageType DamageType { get; }
        public int BaseDamage { get; }

        public Observable<DamageContext> OnDealDamage { get; }
    }
}