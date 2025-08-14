using System;
using System.Linq;
using R3;
using VContainer.Unity;

namespace S1LV3Rman.RockFall.CoreGameplay
{
    public sealed class AsteroidsLogic : IInitializable, IDisposable
    {
        private readonly CurrentGameState _currentGameState;
        private readonly AsteroidsFactory _factory;
        private readonly AsteroidSpawner _spawner;
        private readonly SpaceStationsPool _stationsPool;

        private IDisposable _subscriptions;

        public AsteroidsLogic(
            CurrentGameState currentGameState,
            AsteroidsFactory factory,
            AsteroidSpawner spawner,
            SpaceStationsPool stationsPool
        )
        {
            _currentGameState = currentGameState;
            _factory = factory;
            _spawner = spawner;
            _stationsPool = stationsPool;
        }

        public void Initialize()
        {
            var asteroidsTarget = _stationsPool.First();
            _spawner.SetTarget(asteroidsTarget.transform);
            
            var stateChange = _currentGameState.Subscribe(CheckState);
            var asteroidSpawn = _spawner.Requests.Subscribe(request => _factory.CreateAsteroid(request));
            _subscriptions = Disposable.Combine(stateChange, asteroidSpawn);
        }

        private void CheckState(GameState state)
        {
            _spawner.SetActive(state == GameState.InGame);
        }

        public void Dispose()
        {
            _subscriptions.Dispose();
        }
    }
}