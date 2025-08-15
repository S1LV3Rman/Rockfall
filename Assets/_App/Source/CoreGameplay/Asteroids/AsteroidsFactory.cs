using System.Linq;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace S1LV3Rman.RockFall.CoreGameplay
{
    public sealed class AsteroidsFactory : BaseFactory
    {
        private readonly AsteroidsPool _pool;
        private readonly AsteroidsConfig _config;
        private readonly Transform _world;
        private readonly IndicatorsFactory _indicatorsFactory;
        private readonly SpaceStationsPool _stationsPool;
        private readonly RandomService _randomService;

        public AsteroidsFactory(
            AsteroidsPool pool,
            AsteroidsConfig config,
            [Key("World")] Transform world,
            IndicatorsFactory indicatorsFactory,
            SpaceStationsPool stationsPool,
            RandomService randomService,
            LifetimeScope lifetimeScope
        ) : base(lifetimeScope)
        {
            _pool = pool;
            _config = config;
            _world = world;
            _indicatorsFactory = indicatorsFactory;
            _stationsPool = stationsPool;
            _randomService = randomService;
        }

        protected override void Installation(IContainerBuilder builder)
        {
            
        }

        public Asteroid CreateAsteroid(AsteroidSpawnRequest spawnRequest)
        {
            var asteroidPrefab = _config.BasicAsteroid;
            var asteroid = Container.Instantiate(asteroidPrefab, 
                spawnRequest.Position, Quaternion.LookRotation(spawnRequest.Direction), _world);

            var station = _stationsPool.First();
            _indicatorsFactory.CreateIndicator(asteroid, asteroid.IndicatorColor, asteroid.IndicatorSize)
                .WithHealth(asteroid.IndicatorHealthColor)
                .WithDistanceTo(station)
                .WithName(asteroid.Name);

            var velocity = spawnRequest.Direction * spawnRequest.LaunchSpeed;
            var angularVelocity = _randomService.Direction();
            asteroid.Launch(velocity, angularVelocity);
            
            _pool.Add(asteroid);
            return asteroid;
        }
    }
}