using System;
using Cysharp.Threading.Tasks;
using R3;
using S1LV3Rman.RockFall.App;
using S1LV3Rman.RockFall.MainMenu;
using VContainer.Unity;

namespace S1LV3Rman.RockFall.CoreGameplay
{
    public class PauseLogic : IInitializable, IDisposable
    {
        private readonly InGameUI _inGameUI;
        private readonly PauseUI _pauseUI;
        private readonly CurrentGameState _currentGameState;
        private readonly TimeService _timeService;
        private readonly AppStateChanger _appStateChanger;

        private IDisposable _subscriptions;

        public PauseLogic(
            InGameUI inGameUI,
            PauseUI pauseUI,
            CurrentGameState currentGameState,
            TimeService timeService,
            AppStateChanger appStateChanger
        )
        {
            _inGameUI = inGameUI;
            _pauseUI = pauseUI;
            _currentGameState = currentGameState;
            _timeService = timeService;
            _appStateChanger = appStateChanger;
        }

        public void Initialize()
        {
            _subscriptions = Disposable.Combine(
                _inGameUI.PausePress.Subscribe(Pause),
                _pauseUI.ResumePress.Subscribe(Unpause),
                _pauseUI.ToMainMenuPress.Subscribe(EnterMainMenu));
        }

        private void Pause(Unit _)
        {
            _inGameUI.Close();
            _pauseUI.Open();

            _timeService.TimeScale = 0f;
            _currentGameState.Value = GameState.OnPause;
        }

        private void Unpause(Unit _)
        {
            _pauseUI.Close();
            _inGameUI.Open();

            _timeService.TimeScale = 1f;
            _currentGameState.Value = GameState.InGame;
        }

        private void EnterMainMenu(Unit _)
        {
            _timeService.TimeScale = 1f;

            var stateData = new MainMenuStateData(false);
            _appStateChanger.ChangeStateAsync<MainMenuState, MainMenuStateData>(stateData).Forget();
        }

        public void Dispose()
        {
            _subscriptions.Dispose();
        }
    }
}