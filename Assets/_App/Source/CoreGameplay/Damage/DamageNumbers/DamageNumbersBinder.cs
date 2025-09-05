using System;
using R3;
using VContainer.Unity;

namespace S1LV3Rman.RockFall.CoreGameplay
{
    public sealed class DamageNumbersBinder : IInitializable, IDisposable
    {
        private readonly DamageNumbersFactory _factory;
        private readonly AsteroidsPool _asteroidsPool;
        private readonly SpaceStationsPool _spaceStationsPool;

        private IDisposable _sub;

        public DamageNumbersBinder(
            DamageNumbersFactory factory,
            AsteroidsPool asteroidsPool,
            SpaceStationsPool spaceStationsPool
        )
        {
            _factory = factory;
            _asteroidsPool = asteroidsPool;
            _spaceStationsPool = spaceStationsPool;
        }

        public void Initialize()
        {
            var asteroidsSub = _asteroidsPool.OnAdded
                .SelectMany(asteroid => asteroid.Health.OnDamaged)
                .Subscribe(damageEvent => 
                    _factory.CreateDamageNumber(damageEvent.Context.HitPoint, damageEvent.AppliedAmount));
            
            var spaceStationsSub = _spaceStationsPool.OnAdded
                .SelectMany(station => station.Health.OnDamaged)
                .Subscribe(damageEvent => 
                    _factory.CreateDamageNumber(damageEvent.Context.HitPoint, damageEvent.AppliedAmount));

            _sub = Disposable.Combine(asteroidsSub, spaceStationsSub);
        }

        public void Dispose() => _sub?.Dispose();
    }
}