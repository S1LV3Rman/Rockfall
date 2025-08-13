using S1LV3Rman.RockFall.App;
using UnityEngine;

namespace S1LV3Rman.RockFall.CoreGameplay
{
    public class IndicatorManager : Singleton<IndicatorManager>
    {
        [SerializeField] private Camera _mainCamera;
        [SerializeField] private RectTransform _uiContainer;
        [SerializeField] private TargetIndicator _indicatorPrefab;
        [SerializeField] private HealthIndicator _healthIndicatorPrefab;
        [SerializeField] private DistanceIndicator _distanceIndicatorPrefab;
        [SerializeField] private NameIndicator _nameIndicatorPrefab;

        public IndicatorBuilder AddIndicator(Transform target, Color color, float size = 1f, Sprite sprite = null)
        {
            var newIndicator = Instantiate(_indicatorPrefab, _uiContainer, false);
            newIndicator.FollowTarget(target, _mainCamera);
            newIndicator.Color = color;
            newIndicator.Scale *= size;
            if (sprite != null)
                newIndicator.Sprite = sprite;

            return new IndicatorBuilder(
                newIndicator,
                _healthIndicatorPrefab,
                _distanceIndicatorPrefab,
                _nameIndicatorPrefab);
        }
    }
}