using System;
using System.Collections.Generic;
using VContainer.Unity;
using R3;

namespace S1LV3Rman.RockFall.CoreGameplay
{
    public sealed class DamageSystem : IInitializable, IDisposable
    {
        private readonly InstanceRegistry<IDamageableProvider> _damageables;

        private IDisposable _subscription;

        public DamageSystem(
            InstanceRegistry<IDamageableProvider> damageables
            )
        {
            _damageables = damageables;
        }

        public void Initialize()
        {
            _subscription = _damageables.OnRegistered
                .SelectMany(damageable => damageable.Health.OnReceivingDamage)
                .Subscribe(ProcessDamageReceiving);
        }

        private void ProcessDamageReceiving(DamageContext context)
        {
            var incomingDamage = context.Dealer.Modifier.Modify(ref context, context.BaseDamage);
            var appliedDamage = context.Receiver.Modifier.Modify(ref context, incomingDamage);

            context.Receiver.ApplyDamage(context, appliedDamage);
            
            context.Dealer.Modifier.OnApplied(context, appliedDamage);
            context.Receiver.Modifier.OnApplied(context, appliedDamage);
        }

        public void Dispose()
        {
            _subscription.Dispose();
        }
    }
}