using System;
using System.Collections.Generic;

namespace S1LV3Rman.RockFall
{
    public abstract class BasePool<T> : List<T>, IDisposable
        where T : IDisposable
    {
        public event Action<T> BeforeAdd;
        public event Action<T> AfterAdd;
        public event Action<T> BeforeRemove;
        public event Action<T> AfterRemove;
        public event Action<IReadOnlyCollection<T>> BeforeClear;
        public event Action<IReadOnlyCollection<T>> AfterClear;

        public new void Add(T obj)
        {
            BeforeAdd?.Invoke(obj);
            base.Add(obj);
            AfterAdd?.Invoke(obj);
        }

        public new void Remove(T obj)
        {
            BeforeRemove?.Invoke(obj);
            obj.Dispose();
            base.Remove(obj);
            AfterRemove?.Invoke(obj);
        }

        public new void RemoveAt(int index)
        {
            var obj = this[index];
            BeforeRemove?.Invoke(obj);
            obj.Dispose();
            base.RemoveAt(index);
            AfterRemove?.Invoke(obj);
        }

        public new void Clear()
        {
            var objs = ToArray();
            foreach (var obj in this)
                obj.Dispose();
            BeforeClear?.Invoke(objs);
            base.Clear();
            AfterClear?.Invoke(objs);
        }

        public void Dispose()
        {
            Clear();
        }
    }
}