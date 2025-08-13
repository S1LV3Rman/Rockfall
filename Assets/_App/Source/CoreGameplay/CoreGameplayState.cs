using Eflatun.SceneReference;

namespace S1LV3Rman.RockFall
{
    public class CoreGameplayState : SingleSceneAppState<CoreGameplayLifetimeScope, CoreGameplayStateData>
    {
        private readonly AppScenesConfig _appScenesConfig;

        public CoreGameplayState(
            AppScenesConfig appScenesConfig,
            SceneChanger sceneChanger
        ) : base(
            sceneChanger
        )
        {
            _appScenesConfig = appScenesConfig;
        }

        protected override SceneReference RequiredScene => _appScenesConfig.CoreGameplay;
    }
}