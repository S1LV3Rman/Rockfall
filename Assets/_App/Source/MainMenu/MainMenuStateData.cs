using S1LV3Rman.RockFall.App;

namespace S1LV3Rman.RockFall.MainMenu
{
    public struct MainMenuStateData : IStateData
    {
        public readonly bool IsRetrying;

        public MainMenuStateData(bool isRetrying) : this()
        {
            IsRetrying = isRetrying;
        }
    }
}