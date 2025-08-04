using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Scripts
{
    public class IndicatorBuilder
    {
        private readonly TargetIndicator _indicator;
        
        private readonly HealthIndicator _healthIndicatorPrefab;
        private readonly DistanceIndicator _distanceIndicatorPrefab;
        private readonly NameIndicator _nameIndicatorPrefab;

        public IndicatorBuilder(
            TargetIndicator indicator,
            HealthIndicator healthIndicatorPrefab,
            DistanceIndicator distanceIndicatorPrefab,
            NameIndicator nameIndicatorPrefab
            )
        {
            _indicator = indicator;
            _healthIndicatorPrefab = healthIndicatorPrefab;
            _distanceIndicatorPrefab = distanceIndicatorPrefab;
            _nameIndicatorPrefab = nameIndicatorPrefab;
        }

        public IndicatorBuilder WithHealth(Color color, Sprite sprite = null)
        {
            if (_indicator.HasCustomSprite && sprite != null)
                throw new ArgumentException("Health sprite should be specified if main sprite was changed",
                    nameof(sprite));

            var damageTaking = _indicator.Target.GetComponent<DamageTaking>();
            if (damageTaking == null)
                throw new InvalidOperationException(
                    "Health indicator can't be added to target without " + nameof(DamageTaking));

            var healthIndicator = Object.Instantiate(_healthIndicatorPrefab, _indicator.transform);
            healthIndicator.SetTarget(damageTaking);
            healthIndicator.SetColor(color);
            if (sprite != null)
                healthIndicator.SetSprite(sprite);

            return this;
        }

        public IndicatorBuilder WithDistance(Transform targetForDistance)
        {
            var distanceIndicator = Object.Instantiate(_distanceIndicatorPrefab, _indicator.transform);
            distanceIndicator.SetDistanceTargets(_indicator.Target, targetForDistance);
            distanceIndicator.SetColor(_indicator.Color);
            return this;
        }

        public IndicatorBuilder WithName(string name)
        {
            var nameIndicator = Object.Instantiate(_nameIndicatorPrefab, _indicator.transform);
            nameIndicator.SetName(name);
            nameIndicator.SetColor(_indicator.Color);
            return this;
        }
    }
}