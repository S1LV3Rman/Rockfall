using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace S1LV3Rman.RockFall.CoreGameplay
{
    public class IndicatorBuilder
    {
        private readonly Indicator _indicator;
        private readonly IndicatorsConfig _config;
        private readonly IObjectResolver _container;

        private bool _isColored;
        private Color _color;
        private bool _hasCustomSprite;
        private bool _hasCustomHealthSprite;

        private TargetIndicator _targetIndicator;
        private HealthIndicator _healthIndicator;
        private DistanceIndicator _distanceIndicator;
        private NameIndicator _nameIndicator;

        public IndicatorBuilder(
            Func<Indicator> indicatorProvider,
            IndicatorsConfig config,
            IObjectResolver container
        )
        {
            _indicator = indicatorProvider.Invoke();
            _config = config;
            _container = container;
        }

        public IndicatorBuilder Colored(Color color)
        {
            _color = color;
            _indicator.SetColor(color);
            _distanceIndicator?.SetColor(color);
            _nameIndicator?.SetColor(color);
            _isColored = true;
            return this;
        }

        public IndicatorBuilder OfSize(float size)
        {
            _indicator.SetNormalSize(size);
            return this;
        }

        public IndicatorBuilder WithCustomSprite(Sprite sprite)
        {
            if (_healthIndicator != null && !_hasCustomHealthSprite)
                throw new InvalidOperationException("Health sprite should be specified if main sprite was changed");

            _indicator.SetSprite(sprite);
            _hasCustomSprite = true;
            return this;
        }

        public IndicatorBuilder WithTargetFollowing(AliveTrackedMonoBehaviour target)
        {
            var targetIndicator = _container.Instantiate(_config.TargetIndicatorPrefab, _indicator.transform);
            targetIndicator.FollowTarget(target);
            _indicator.AddModification(targetIndicator);
            return this;
        }

        public IndicatorBuilder WithHealth(AliveTrackedMonoBehaviour target, Color color, Sprite sprite = null)
        {
            if (_hasCustomSprite && sprite == null)
                throw new ArgumentException("Health sprite should be specified if main sprite was changed",
                    nameof(sprite));

            var damageTaking = target.GetComponent<Health>();
            if (damageTaking == null)
                throw new InvalidOperationException(
                    "Health indicator can't be added to target without " + nameof(Health));

            var healthIndicator = _container.Instantiate(_config.HealthIndicatorPrefab, _indicator.transform);
            healthIndicator.SetTarget(damageTaking);
            healthIndicator.SetColor(color);
            if (sprite != null)
            {
                healthIndicator.SetSprite(sprite);
                _hasCustomHealthSprite = true;
            }

            _indicator.AddModification(healthIndicator);
            return this;
        }

        public IndicatorBuilder WithDistance(AliveTrackedMonoBehaviour fromTarget, AliveTrackedMonoBehaviour toTarget)
        {
            var distanceIndicator = _container.Instantiate(_config.DistanceIndicatorPrefab, _indicator.transform);
            distanceIndicator.SetDistanceTargets(fromTarget, toTarget);
            if (_isColored)
                distanceIndicator.SetColor(_color);

            _indicator.AddModification(distanceIndicator);
            return this;
        }

        public IndicatorBuilder WithName(string name)
        {
            var nameIndicator = _container.Instantiate(_config.NameIndicatorPrefab, _indicator.transform);
            nameIndicator.SetName(name);
            if (_isColored)
                nameIndicator.SetColor(_color);

            _indicator.AddModification(nameIndicator);
            return this;
        }
    }
}