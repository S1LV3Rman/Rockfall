using Eflatun.SceneReference;

namespace S1LV3Rman.RockFall
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