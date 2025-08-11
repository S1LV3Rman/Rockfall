using S1LV3Rman.RockFall;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace UneasyPixel.Game
{
    public class CoreGameplayLifetimeScope : LifetimeScope
    {
        [Header("Scene")]
        [SerializeField] private GameplayCamera _camera;

        // [Header("UI")]
        
        // [Header("Configs")]

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(_camera);
            
            // builder.RegisterInstance(_gameMenu);
            //
            // builder.RegisterInstance(_charactersConfig);
            // builder.RegisterInstance(_worldConfig);
            // builder.RegisterInstance(_itemsConfig);
            //
            // builder.Register<CharactersFactory>(Lifetime.Singleton);
            // builder.Register<CharactersPool>(Lifetime.Singleton);
            //
            // builder.Register<SubscriptionsFactory>(Lifetime.Singleton);
            // builder.RegisterEntryPoint<PlayerSubscriptionsRunner>().AsSelf();
            //
            // builder.RegisterEntryPoint<WorldCreator>();
            // builder.RegisterEntryPoint<CharactersRunner>();
            // builder.RegisterEntryPoint<Cheats>();
            // builder.RegisterEntryPoint<CameraController>();
            // builder.RegisterEntryPoint<PlayersController>();
            // builder.RegisterEntryPoint<GameMenuController>();
        }
    }
}