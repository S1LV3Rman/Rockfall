using System;
using R3;
using VContainer.Unity;

namespace S1LV3Rman.RockFall.CoreGameplay
{
    public sealed class DamageSystem : IInitializable, IDisposable
    {
        private readonly InstanceRegistry<IDamageable> _damageables;
        private readonly InstanceRegistry<IDamageDealer> _damageDealers;

        private IDisposable _subscription;

        public DamageSystem(
            InstanceRegistry<IDamageable> damageables,
            InstanceRegistry<IDamageDealer> damageDealers
            )
        {
            _damageables = damageables;
            _damageDealers = damageDealers;
        }

        public void Initialize()
        {
            _subscription = _damageDealers.OnRegistered
                .SelectMany(damageDealer => damageDealer.OnDealDamage)
                .Subscribe(ProcessDamageDealing);
        }

        private void ProcessDamageDealing(DamageContext context)
        {
            // var incomingDamage = context.Dealer.Modifier.Modify(ref context, context.BaseDamage);
            // var appliedDamage = context.Receiver.Modifier.Modify(ref context, incomingDamage);

            context.Receiver.ApplyDamage(context, context.BaseDamage);
            
            // context.Dealer.Modifier.OnApplied(context, appliedDamage);
            // context.Receiver.Modifier.OnApplied(context, appliedDamage);
        }

        public void Dispose()
        {
            _subscription.Dispose();
        }
    }
}