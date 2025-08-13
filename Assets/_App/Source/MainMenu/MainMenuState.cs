using Eflatun.SceneReference;
using S1LV3Rman.RockFall.App;

namespace S1LV3Rman.RockFall.MainMenu
{
    public class MainMenuState : SingleSceneAppState<MainMenuLifetimeScope, MainMenuStateData>
    {
        private readonly AppScenesConfig _appScenesConfig;

        public MainMenuState(
            AppScenesConfig appScenesConfig,
            SceneChanger sceneChanger
        ) : base(
            sceneChanger
        )
        {
            _appScenesConfig = appScenesConfig;
        }

        protected override SceneReference RequiredScene => _appScenesConfig.MainMenu;
    }
}