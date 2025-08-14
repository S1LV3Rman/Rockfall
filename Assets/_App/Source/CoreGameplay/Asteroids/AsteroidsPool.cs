using System;
using R3;

namespace S1LV3Rman.RockFall.CoreGameplay
{
    public sealed class AsteroidsPool : BasePool<Asteroid>
    {
        private readonly IDisposable _subscription;

        public AsteroidsPool()
        {
            _subscription = OnAdded.SelectMany(asteroid =>
                    asteroid.IsAlive
                        .Where(isAlive => !isAlive)
                        .Take(1)
                        .TakeUntil(OnRemoved.Where(r => ReferenceEquals(r, asteroid)))
                        .Do(_ => Remove(asteroid)))
                .Subscribe();
        }

        public override void Dispose()
        {
            base.Dispose();
            _subscription.Dispose();
        }
    }
}