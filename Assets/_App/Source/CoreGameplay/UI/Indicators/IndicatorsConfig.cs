using UnityEngine;

namespace S1LV3Rman.RockFall.CoreGameplay
{
    [CreateAssetMenu(fileName = nameof(IndicatorsConfig), menuName = "Config/" + nameof(IndicatorsConfig), order = 0)]
    public sealed class IndicatorsConfig : ScriptableObject
    {
        [field: SerializeField] public TargetIndicator IndicatorPrefab { get; private set; }
        [field: SerializeField] public HealthIndicator HealthIndicatorPrefab { get; private set; }
        [field: SerializeField] public DistanceIndicator DistanceIndicatorPrefab { get; private set; }
        [field: SerializeField] public NameIndicator NameIndicatorPrefab { get; private set; }
    }
}