using System;
using R3;
using R3.Triggers;
using UnityEngine;

namespace S1LV3Rman.RockFall
{
    public class AliveTrackedMonoBehaviour : MonoBehaviour, IDisposable
    {
        public ReadOnlyReactiveProperty<bool> IsAlive { get; private set; }

        protected virtual void Awake()
        {
            IsAlive = Observable.Merge(
                    Observable.Return(true), // initial state
                    this.OnDestroyAsObservable().Select(_ => false))
                .ToReadOnlyReactiveProperty();
        }

        public void Destroy()
        {
            if (IsAlive.CurrentValue)
                Destroy(gameObject);
        }

        public virtual void Dispose()
        {
            Destroy();
        }
    }
}