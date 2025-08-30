using System;
using R3;
using VContainer.Unity;

namespace S1LV3Rman.RockFall.CoreGameplay
{
    public class BoundaryLogic : IInitializable, ITickable, IDisposable
    {
        private readonly SpaceShip _spaceShip;
        private readonly SpaceStation _spaceStation;
        private readonly Boundary _boundary;
        private readonly BoundaryWarningUI _warningUI;
        private readonly CurrentGameState _currentGameState;

        private IDisposable _subscriptions;

        public BoundaryLogic(
            SpaceShip spaceShip,
            SpaceStation spaceStation,
            Boundary boundary,
            BoundaryWarningUI warningUI,
            CurrentGameState currentGameState
            )
        {
            _spaceShip = spaceShip;
            _spaceStation = spaceStation;
            _boundary = boundary;
            _warningUI = warningUI;
            _currentGameState = currentGameState;
        }

        public void Initialize()
        {
            _subscriptions = _currentGameState.Subscribe(CheckState);
        }

        private void CheckState(GameState state)
        {
            if (state != GameState.InGame)
                _warningUI.Close();
        }

        public void Tick()
        {
            if (_currentGameState.CurrentValue != GameState.InGame)
                return;
            
            var distance = (_spaceShip.transform.position - _spaceStation.transform.position).magnitude;

            if (distance > _boundary.DestroyRadius)
                _currentGameState.Value = GameState.GameOver;
            else
                _warningUI.SetOpened(distance > _boundary.WarningRadius);
        }

        public void Dispose()
        {
            _subscriptions.Dispose();
        }
    }
}