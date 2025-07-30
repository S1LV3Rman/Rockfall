using UnityEngine;

namespace Scripts
{
    public class IndicatorManager : Singleton<IndicatorManager>
    {
        [SerializeField] private Camera _mainCamera;
        [SerializeField] private RectTransform _uiContainer;
        [SerializeField] private TargetIndicator _indicatorPrefab;
        [SerializeField] private HealthIndicator _healthIndicatorPrefab;
        [SerializeField] private DistanceIndicator _distanceIndicatorPrefab;

        public IndicatorBuilder AddIndicator(Transform target, Color color, Sprite sprite = null)
        {
            var newIndicator = Instantiate(_indicatorPrefab, _uiContainer, false);
            newIndicator.FollowTarget(target, _mainCamera);
            newIndicator.Color = color;
            if (sprite != null)
                newIndicator.Sprite = sprite;

            return new IndicatorBuilder(newIndicator, sprite != null,
                _healthIndicatorPrefab, _distanceIndicatorPrefab);
        }
    }
}