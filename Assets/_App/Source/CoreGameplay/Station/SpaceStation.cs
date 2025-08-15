using UnityEngine;

namespace S1LV3Rman.RockFall.CoreGameplay
{
    public class SpaceStation : AliveTrackedMonoBehaviour
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public Color IndicatorColor { get; private set; }
        [field: SerializeField] public Color IndicatorHealthColor { get; private set; }
        [field: SerializeField] public float IndicatorSize { get; private set; } = 2f;
    }
}