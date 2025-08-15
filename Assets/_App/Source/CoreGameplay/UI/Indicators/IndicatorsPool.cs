using System;
using R3;

namespace S1LV3Rman.RockFall.CoreGameplay
{
    public sealed class IndicatorsPool : BasePool<TargetIndicator>
    {
        private readonly IDisposable _subscription;

        public IndicatorsPool()
        {
            _subscription = OnAdded.SelectMany(indicator =>
                    indicator.IsAlive
                        .Where(isAlive => !isAlive)
                        .Take(1)
                        .TakeUntil(OnRemoved.Where(r => ReferenceEquals(r, indicator)))
                        .Do(_ => Remove(indicator)))
                .Subscribe();
        }

        public override void Dispose()
        {
            base.Dispose();
            _subscription.Dispose();
        }
    }
}