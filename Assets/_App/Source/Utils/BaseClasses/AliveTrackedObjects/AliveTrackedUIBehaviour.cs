using System;
using R3;
using R3.Triggers;
using UnityEngine.EventSystems;

namespace S1LV3Rman.RockFall
{
    public class AliveTrackedUIBehaviour : UIBehaviour, IAliveTrackedObject, IDisposable
    {
        public ReadOnlyReactiveProperty<bool> IsAlive { get; private set; }

        protected override void Awake()
        {
            IsAlive = Observable.Merge(
                    Observable.Return(true), // initial state
                    this.OnDestroyAsObservable().Select(_ => false))
                .ToReadOnlyReactiveProperty();
        }

        private void Destroy()
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