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
        private readonly InGameUI _inGameUI;
        private readonly GameOverUI _gameOverUI;
        private readonly AppStateChanger _appStateChanger;

        private IDisposable _subscriptions;

        public GameOverLogic(
            CurrentGameState currentGameState,
            InGameUI inGameUI,
            GameOverUI gameOverUI,
            AppStateChanger appStateChanger
        )
        {
            _currentGameState = currentGameState;
            _inGameUI = inGameUI;
            _gameOverUI = gameOverUI;
            _appStateChanger = appStateChanger;
        }

        public void Initialize()
        {
            _subscriptions = _currentGameState.Subscribe(CheckGameState);
        }

        private void CheckGameState(GameState state)
        {
            if (state == GameState.GameOver)
                GameOver();
        }

        private void GameOver()
        {
            var returning = _gameOverUI.RetryPress.Subscribe(ReturnToWeaponSelection);
            _subscriptions = Disposable.Combine(_subscriptions, returning);
            
            
            
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