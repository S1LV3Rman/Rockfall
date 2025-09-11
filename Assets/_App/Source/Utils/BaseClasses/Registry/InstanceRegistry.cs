using System;
using System.Collections.Generic;
using R3;

namespace S1LV3Rman.RockFall
{
    public sealed class InstanceRegistry<T> : IDisposable
    {
        private readonly List<T> _instances = new();
        private readonly Subject<T> _onRegistered = new();

        public IReadOnlyList<T> All => _instances;
        public Observable<T> OnRegistered => _onRegistered;

        public void Register(T instance)
        {
            if (_instances.Contains(instance))
                return;

            _instances.Add(instance);
            _onRegistered.OnNext(instance);
        }

        public bool TryRegister<TType>(TType instance)
        {
            if (instance is not T tInstance || _instances.Contains(tInstance))
                return false;

            _instances.Add(tInstance);
            _onRegistered.OnNext(tInstance);
            return true;
        }

        public void Unregister(T instance)
        {
            _instances.Remove(instance);
        }

        public bool TryUnregister<TType>(TType instance) => 
            instance is T tInstance && _instances.Remove(tInstance);

        public void Dispose()
        {
            _onRegistered.OnCompleted();
            _onRegistered.Dispose();
        }
    }
}