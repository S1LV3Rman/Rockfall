using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace S1LV3Rman.RockFall.CoreGameplay
{
    public sealed class IndicatorsFactory : BaseFactory
    {
        private readonly RectTransform _uiContainer;
        private readonly GameplayCamera _camera;
        private readonly IndicatorsConfig _config;
        private readonly IndicatorsPool _pool;

        public IndicatorsFactory(
            [Key("Indicators")] RectTransform uiContainer,
            GameplayCamera camera,
            IndicatorsConfig config,
            IndicatorsPool pool,
            LifetimeScope lifetimeScope
        ) : base(lifetimeScope)
        {
            _uiContainer = uiContainer;
            _camera = camera;
            _config = config;
            _pool = pool;
        }

        protected override void Installation(IContainerBuilder builder)
        {
        }

        public IndicatorBuilder CreateIndicator(AliveTrackedMonoBehaviour target,
            Color color, float size = 1f, Sprite sprite = null)
        {
            var newIndicator = Container.Instantiate(_config.IndicatorPrefab, _uiContainer);
            newIndicator.FollowTarget(target, _camera.Camera);
            newIndicator.Color = color;
            newIndicator.Scale *= size;
            if (sprite != null)
                newIndicator.Sprite = sprite;

            _pool.Add(newIndicator);
            
            return new IndicatorBuilder(
                newIndicator,
                _config.HealthIndicatorPrefab,
                _config.DistanceIndicatorPrefab,
                _config.NameIndicatorPrefab);
        }
    }
}