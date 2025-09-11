using System;
using R3;
using VContainer.Unity;

namespace S1LV3Rman.RockFall.CoreGameplay
{
    public sealed class DamageNumbersSystem : IInitializable, IDisposable
    {
        private readonly DamageNumbersFactory _factory;
        private readonly InstanceRegistry<IDamageableProvider> _damageables;

        private IDisposable _subscription;

        public DamageNumbersSystem(
            DamageNumbersFactory factory,
            InstanceRegistry<IDamageableProvider> damageables)
        {
            _factory = factory;
            _damageables = damageables;
        }

        public void Initialize()
        {
            _subscription = _damageables.OnRegistered
                .SelectMany(damageable => damageable.Health.OnDamaged)
                .Subscribe(damageEvent =>
                    _factory.CreateDamageNumber(damageEvent.Context.HitPoint, damageEvent.AppliedAmount));
        }

        public void Dispose() => _subscription.Dispose();
    }
}