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
        private readonly RandomService _randomService;

        public AsteroidsFactory(
            AsteroidsPool pool,
            AsteroidsConfig config,
            [Key("World")] Transform world,
            RandomService randomService,
            LifetimeScope lifetimeScope
        ) : base(lifetimeScope)
        {
            _pool = pool;
            _config = config;
            _world = world;
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

            var velocity = spawnRequest.Direction * spawnRequest.LaunchSpeed;
            var angularVelocity = _randomService.Direction();
            asteroid.Launch(velocity, angularVelocity);
            
            _pool.Add(asteroid);
            return asteroid;
        }
    }
}