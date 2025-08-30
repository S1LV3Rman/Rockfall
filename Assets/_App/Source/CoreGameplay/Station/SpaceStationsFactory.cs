using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace S1LV3Rman.RockFall.CoreGameplay
{
    public class SpaceStationsFactory : BaseFactory
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
                .WithHealth(station, station.IndicatorHealthColor)
                .WithName(station.Name)
                .Colored(station.IndicatorColor)
                .OfSize(station.IndicatorSize);
            
            return station;
        }
    }
}