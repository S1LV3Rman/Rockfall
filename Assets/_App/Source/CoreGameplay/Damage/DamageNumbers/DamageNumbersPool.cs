using System;
using System.Collections.Generic;
using R3;

namespace S1LV3Rman.RockFall.CoreGameplay
{
    public sealed class DamageNumbersPool : AliveObjectsPool<DamageNumber>
    {
        private readonly Dictionary<DamageNumber, IDisposable> _subscriptions = new();

        public override void Add(DamageNumber damageNumber)
        {
            base.Add(damageNumber);
            _subscriptions[damageNumber] =
                damageNumber.LifetimeExpiration.Subscribe(Release);
        }

        public override bool Remove(DamageNumber damageNumber)
        {
            var wasRemoved = base.Remove(damageNumber);
            if (wasRemoved && _subscriptions.TryGetValue(damageNumber, out var sub))
            {
                sub.Dispose();
                _subscriptions.Remove(damageNumber);
            }
            return wasRemoved;
        }

        public override void Dispose()
        {
            foreach (var sub in _subscriptions.Values)
                sub.Dispose();

            _subscriptions.Clear();
            base.Dispose();
        }
    }
}