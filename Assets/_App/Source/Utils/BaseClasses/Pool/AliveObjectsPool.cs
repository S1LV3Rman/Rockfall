using System;
using System.Collections.Generic;
using R3;

namespace S1LV3Rman.RockFall
{
    // todo: не уверен, что хочу полагаться на реактивщину для удаления объектов из пулла. Может лучше убирать их вручную
    public abstract class AliveObjectsPool<T> : BasePool<T> where T : IAliveTrackedObject, IReusableInPool, IDisposable
    {
        private readonly Dictionary<T, IDisposable> _subscriptions = new();

        public override void Add(T item)
        {
            base.Add(item);
            var sub = item.IsAlive
                .Where(isAlive => !isAlive)
                .Take(1)
                .Subscribe(_ => Remove(item));

            _subscriptions[item] = sub;
        }

        public override bool Remove(T item)
        {
            var wasRemoved = base.Remove(item);
            if (wasRemoved && _subscriptions.TryGetValue(item, out var sub))
            {
                sub.Dispose();
                _subscriptions.Remove(item);
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