using R3;

namespace S1LV3Rman.RockFall.CoreGameplay
{
    public class CurrentGameState : ReactiveProperty<GameState>
    {
        public CurrentGameState() : base(GameState.Undefined)
        {
        }
    }
    
    public enum GameState
    {
        Undefined,
        InGame,
        OnPause,
        GameOver
    }
}