using S1LV3Rman.RockFall.App;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace S1LV3Rman.RockFall.CoreGameplay
{
    public sealed class CoreGameplayLifetimeScope : AppStateLifetimeScope<CoreGameplayStateData>
    {
        [Header("Scene")]
        [SerializeField] private GameplayCamera _camera;
        [SerializeField] private Transform _stationStartPoint;
        [SerializeField] private Transform _shipStartPoint;

        // [Header("UI")]

        [Header("Configs")]
        [SerializeField] private SpaceShipsConfig _spaceShipsConfig;
        [SerializeField] private SpaceStationsConfig _spaceStationsConfig;
        [SerializeField] private WeaponsConfig _weaponsConfig;

        protected override void ConfigureState(IContainerBuilder builder)
        {
            builder.RegisterInstance(_camera);

            builder.RegisterInstance(_stationStartPoint).Keyed("StationStart");
            builder.RegisterInstance(_shipStartPoint).Keyed("ShipStart");
            
            builder.RegisterInstance(_spaceShipsConfig);
            builder.RegisterInstance(_spaceStationsConfig);
            builder.RegisterInstance(_weaponsConfig);

            builder.Register<CurrentGameState>(Lifetime.Singleton);

            builder.RegisterEntryPoint<CoreGameplayLogic>();

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