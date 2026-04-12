using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace S1LV3Rman.RockFall.CoreGameplay
{
    public class SpaceStationsFactory : Factory
    {
        private readonly SpaceStationsPool _pool;
        private readonly SpaceStationsConfig _config;
        private readonly Transform _world;
        private readonly Transform _startingPoint;
        private readonly IndicatorsFactory _indicatorsFactory;

        public SpaceStationsFactory(
            SpaceStationsPool pool,
            SpaceStationsConfig config,
            [Key("World")] Transform world,
            [Key("SpaceStation")] Transform startingPoint,
            IndicatorsFactory indicatorsFactory,
            LifetimeScope lifetimeScope
        ) : base(lifetimeScope)
        {
            _pool = pool;
            _config = config;
            _world = world;
            _startingPoint = startingPoint;
            _indicatorsFactory = indicatorsFactory;
        }

        protected override void Installation(IContainerBuilder builder)
        {
        }

        public SpaceStation CreateBasicStation()
        {
            var station = Container.Instantiate(
                _config.BasicStation, _startingPoint.position, _startingPoint.rotation, _world);
            _pool.Add(station);
            
            _indicatorsFactory.CreateIndicator()
                .WithTargetFollowing(station)
                .WithHealth(station, _config.Indicator.HealthColor)
                .WithName(_config.Indicator.Name)
                .Colored(_config.Indicator.Color)
                .OfSize(_config.Indicator.Size);
            
            return station;
        }
    }
}