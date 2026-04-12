using System;
using System.Collections.Generic;
using R3;

namespace S1LV3Rman.RockFall
{
    public sealed class InstanceRegistry<T> : IDisposable
    {
        private readonly List<T> _registeredInstances = new();
        public IReadOnlyList<T> All => _registeredInstances;
        
        private readonly Subject<T> _onRegistered = new();
        public Observable<T> OnRegistered => _onRegistered;
        
        private readonly Subject<T> _onUnregistered = new();
        public Observable<T> OnUnregistered => _onUnregistered;

        public void Register(T instance)
        {
            if (_registeredInstances.Contains(instance))
                return;

            _registeredInstances.Add(instance);
            _onRegistered.OnNext(instance);
        }

        public bool TryRegister(T instance)
        {
            if (_registeredInstances.Contains(instance))
                return false;

            _registeredInstances.Add(instance);
            _onRegistered.OnNext(instance);
            return true;
        }

        public void Unregister(T instance)
        {
            if (_registeredInstances.Remove(instance))
                _onUnregistered.OnNext(instance);
        }

        public bool TryUnregister(T instance)
        {
            if (!_registeredInstances.Remove(instance))
                return false;
            
            _onUnregistered.OnNext(instance);
            return true;
        }

        public void Dispose()
        {
            _onRegistered.OnCompleted();
            _onRegistered.Dispose();
        }
    }
}