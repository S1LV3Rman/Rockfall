using Cysharp.Threading.Tasks;
using S1LV3Rman.RockFall.MainMenu;
using VContainer.Unity;

namespace S1LV3Rman.RockFall.App
{
    public class AppInitializer : IInitializable
    {
        private readonly AppStateChanger _appStateChanger;

        public AppInitializer(
            AppStateChanger appStateChanger
            )
        {
            _appStateChanger = appStateChanger;
        }

        public void Initialize()
        {
            var stateData = new MainMenuStateData(false);
            _appStateChanger.ChangeStateAsync<MainMenuState, MainMenuStateData>(stateData).Forget();
        }
    }
}