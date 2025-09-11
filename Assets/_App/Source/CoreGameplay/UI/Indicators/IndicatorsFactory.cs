using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace S1LV3Rman.RockFall.CoreGameplay
{
    public sealed class IndicatorsFactory : Factory
    {
        private readonly RectTransform _uiContainer;
        private readonly GameplayCamera _gameplayCamera;
        private readonly IndicatorsConfig _config;
        private readonly IndicatorsPool _pool;

        public IndicatorsFactory(
            [Key("Indicators")] RectTransform uiContainer,
            GameplayCamera gameplayCamera,
            IndicatorsConfig config,
            IndicatorsPool pool,
            LifetimeScope lifetimeScope
        ) : base(lifetimeScope)
        {
            _uiContainer = uiContainer;
            _gameplayCamera = gameplayCamera;
            _config = config;
            _pool = pool;
        }

        protected override void Installation(IContainerBuilder builder)
        {
            builder.RegisterFactory(PullOrCreateIndicator);
            builder.RegisterInstance(_gameplayCamera.Camera);
            builder.Register<IndicatorBuilder>(Lifetime.Transient);
        }

        public IndicatorBuilder CreateIndicator() => Container.Resolve<IndicatorBuilder>();

        private Indicator PullOrCreateIndicator()
        {
            if (_pool.TryPull(out var indicator))
                return indicator;

            var newIndicator = Container.Instantiate(_config.IndicatorPrefab, _uiContainer);
            _pool.Add(newIndicator);
            return newIndicator;
        }
    }
}