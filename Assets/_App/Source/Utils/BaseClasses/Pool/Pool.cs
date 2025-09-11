using System;
using System.Collections.Generic;

namespace S1LV3Rman.RockFall
{
    public abstract class Pool<T> : DisposableList<T> where T : IReusableInPool, IDisposable
    {
        private readonly List<T> _releasedItems = new();

        public virtual bool TryPull(out T item)
        {
            var inPool = _releasedItems.Count;
            if (inPool > 0)
            {
                item = _releasedItems[inPool - 1];
                _releasedItems.RemoveAt(inPool - 1);
                item.PrepareForPulling();
                return true;
            }

            item = default;
            return false;
        }

        public virtual void Release(T item)
        {
            item.PrepareForReleasing();
            _releasedItems.Add(item);
        }
    }
}