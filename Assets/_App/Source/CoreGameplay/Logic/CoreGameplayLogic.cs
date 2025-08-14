using VContainer.Unity;

namespace S1LV3Rman.RockFall.CoreGameplay
{
    public sealed class CoreGameplayLogic : IInitializable
    {
        private readonly CurrentGameState _currentGameState;
        private readonly CoreGameplayStateData _stateData;
        private readonly SpaceShipsFactory _shipsFactory;
        private readonly SpaceStationsFactory _stationsFactory;

        private SpaceShip _currentShip;
        private SpaceStation _currentStation;

        public CoreGameplayLogic(
            CurrentGameState currentGameState,
            CoreGameplayStateData stateData,
            SpaceShipsFactory shipsFactory,
            SpaceStationsFactory stationsFactory
            )
        {
            _currentGameState = currentGameState;
            _stateData = stateData;
            _shipsFactory = shipsFactory;
            _stationsFactory = stationsFactory;
        }

        public void Initialize()
        {
            _currentShip = _shipsFactory.CreateBasicShip(_stateData.WeaponType);
            _currentStation = _stationsFactory.CreateBasicStation();
            _currentGameState.Value = GameState.InGame;
        }
    }
}