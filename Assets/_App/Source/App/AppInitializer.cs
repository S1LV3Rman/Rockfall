using Cysharp.Threading.Tasks;
using VContainer.Unity;

namespace S1LV3Rman.RockFall
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
            var stateData = new MainMenuStateData();
            _appStateChanger.ChangeStateAsync<MainMenuState, MainMenuStateData>(stateData).Forget();
        }
    }
}