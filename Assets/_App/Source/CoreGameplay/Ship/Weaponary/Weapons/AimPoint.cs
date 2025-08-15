using UnityEngine;

namespace S1LV3Rman.RockFall.CoreGameplay
{
    public sealed class AimPoint : AliveTrackedMonoBehaviour
    {
        [field: SerializeField] public Color IndicatorColor { get; private set; }
        [field: SerializeField] public Sprite IndicatorImage { get; private set; }
        [field: SerializeField] public float IndicatorSize { get; private set; } = 0.75f;
    }
}