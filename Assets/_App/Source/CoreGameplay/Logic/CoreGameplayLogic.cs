using VContainer.Unity;

namespace S1LV3Rman.RockFall.CoreGameplay
{
    public sealed class CoreGameplayLogic : IInitializable
    {
        private readonly CurrentGameState _currentGameState;
        private readonly CoreGameplayStateData _stateData;

        public CoreGameplayLogic(
            CurrentGameState currentGameState,
            CoreGameplayStateData stateData
            )
        {
            _currentGameState = currentGameState;
            _stateData = stateData;
        }

        public void Initialize()
        {
            _currentGameState.Value = GameState.InGame;
        }
    }
}