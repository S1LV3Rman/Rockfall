using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace S1LV3Rman.RockFall
{
    public sealed class AppLifetimeScope : LifetimeScope
    {
        [Header("Configs")]
        [SerializeField] private AppScenesConfig _appScenesConfig;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(_appScenesConfig);
            
            builder.Register<MainMenuState>(Lifetime.Transient);
            builder.Register<CoreGameplayState>(Lifetime.Transient);
            
            builder.Register<RandomService>(Lifetime.Singleton);
            builder.Register<TimeService>(Lifetime.Singleton);
            
            builder.Register<SceneChanger>(Lifetime.Singleton);
            builder.Register<AppStateChanger>(Lifetime.Singleton);

            builder.RegisterEntryPoint<AppInitializer>();
        }
    }
}