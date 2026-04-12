using System;
using S1LV3Rman.RockFall.CoreGameplay;

namespace S1LV3Rman.RockFall
{
    public abstract class AliveObjectsPoolWithRegistry<T> : AliveObjectsPool<T> 
        where T : IAliveTrackedObject, IReusableInPool, IDisposable
    {
        public readonly InstanceRegistry<T> ActiveObjects;

        protected AliveObjectsPoolWithRegistry()
        {
            ActiveObjects = new InstanceRegistry<T>();
        }

        public override void Add(T item)
        {
            base.Add(item);
            ActiveObjects.TryRegister(item);
        }

        public override bool Remove(T item)
        {
            var wasRemoved = base.Remove(item);
            if (wasRemoved)
                ActiveObjects.TryUnregister(item);

            return wasRemoved;
        }

        public override bool TryPull(out T item)
        {
            var wasPulled = base.TryPull(out item);
            if (wasPulled)
                ActiveObjects.TryRegister(item);

            return wasPulled;
        }

        public override void Release(T item)
        {
            base.Release(item);
            ActiveObjects.TryUnregister(item);
        }

        public override void Dispose()
        {
            foreach (var item in _items) 
                ActiveObjects.TryUnregister(item);
            ActiveObjects.Dispose();
            base.Dispose();
        }
    }
}