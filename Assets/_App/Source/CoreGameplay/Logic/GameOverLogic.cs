using System;
using Cysharp.Threading.Tasks;
using R3;
using S1LV3Rman.RockFall.App;
using S1LV3Rman.RockFall.MainMenu;
using VContainer.Unity;

namespace S1LV3Rman.RockFall.CoreGameplay
{
    public class GameOverLogic : IInitializable, IDisposable
    {
        private readonly CurrentGameState _currentGameState;
        private readonly SpaceShipsPool _shipsPool;
        private readonly SpaceStationsPool _stationsPool;
        private readonly AsteroidsPool _asteroidsPool;
        private readonly InGameUI _inGameUI;
        private readonly GameOverUI _gameOverUI;
        private readonly AppStateChanger _appStateChanger;

        private IDisposable _subscriptions;

        public GameOverLogic(
            CurrentGameState currentGameState,
            SpaceShipsPool shipsPool,
            SpaceStationsPool stationsPool,
            AsteroidsPool asteroidsPool,
            InGameUI inGameUI,
            GameOverUI gameOverUI,
            AppStateChanger appStateChanger
        )
        {
            _currentGameState = currentGameState;
            _shipsPool = shipsPool;
            _stationsPool = stationsPool;
            _asteroidsPool = asteroidsPool;
            _inGameUI = inGameUI;
            _gameOverUI = gameOverUI;
            _appStateChanger = appStateChanger;
        }

        public void Initialize()
        {
            _subscriptions = _currentGameState
                .Where(state => state == GameState.GameOver)
                .Subscribe(_ => GameOver());
        }

        private void GameOver()
        {
            var returning = _gameOverUI.RetryPress.Subscribe(ReturnToWeaponSelection);
            _subscriptions = Disposable.Combine(_subscriptions, returning);

            _shipsPool.Clear();
            _stationsPool.Clear();
            _asteroidsPool.Clear();

            _inGameUI.Close();
            _gameOverUI.Open();
        }

        private void ReturnToWeaponSelection(Unit _)
        {
            var stateData = new MainMenuStateData(true);
            _appStateChanger.ChangeStateAsync<MainMenuState, MainMenuStateData>(stateData).Forget();
        }

        public void Dispose()
        {
            _subscriptions.Dispose();
        }
    }
}