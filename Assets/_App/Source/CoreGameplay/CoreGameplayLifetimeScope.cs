using S1LV3Rman.RockFall;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace UneasyPixel.Game
{
    public sealed class CoreGameplayLifetimeScope : LifetimeScope
    {
        [Header("Scene")]
        [SerializeField] private GameplayCamera _camera;
        [SerializeField][Key("station")] private Transform _stationStartPoint;
        [SerializeField][Key("ship")] private Transform _shipStartPoint;

        // [Header("UI")]

        [Header("Configs")]
        [SerializeField] private SpaceShipsConfig _spaceShipsConfig;
        [SerializeField] private SpaceStationsConfig _spaceStationsConfig;
        [SerializeField] private WeaponsConfig _weaponsConfig;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(_camera);

            builder.RegisterInstance(_stationStartPoint);
            builder.RegisterInstance(_shipStartPoint);
            
            builder.RegisterInstance(_spaceShipsConfig);
            builder.RegisterInstance(_spaceStationsConfig);
            builder.RegisterInstance(_weaponsConfig);

            // builder.RegisterInstance(_gameMenu);
            //
            // builder.Register<CharactersFactory>(Lifetime.Singleton);
            // builder.Register<CharactersPool>(Lifetime.Singleton);
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