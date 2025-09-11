using System;
using S1LV3Rman.RockFall.CoreGameplay;

namespace S1LV3Rman.RockFall
{
    public abstract class AliveObjectsPoolWithRegistry<T> : AliveObjectsPool<T> 
        where T : IAliveTrackedObject, IReusableInPool, IDisposable
    {
        private readonly InstanceRegistry<IDamageableProvider> _damageables;

        protected AliveObjectsPoolWithRegistry(
            InstanceRegistry<IDamageableProvider> damageables)
        {
            _damageables = damageables;
        }

        public override void Add(T item)
        {
            base.Add(item);
            _damageables.TryRegister(item);
        }

        public override bool Remove(T item)
        {
            var wasRemoved = base.Remove(item);
            if (wasRemoved)
                _damageables.TryUnregister(item);

            return wasRemoved;
        }

        public override bool TryPull(out T item)
        {
            var wasPulled = base.TryPull(out item);
            if (wasPulled)
                _damageables.TryRegister(item);

            return wasPulled;
        }

        public override void Release(T item)
        {
            base.Release(item);
            _damageables.TryUnregister(item);
        }

        public override void Dispose()
        {
            foreach (var item in _items) 
                _damageables.TryUnregister(item);
            base.Dispose();
        }
    }
}