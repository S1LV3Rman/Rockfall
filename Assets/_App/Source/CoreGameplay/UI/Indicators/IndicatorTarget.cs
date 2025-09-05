using UnityEngine;

namespace S1LV3Rman.RockFall.CoreGameplay
{
    public sealed class IndicatorTarget : MonoBehaviour
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public Color Color { get; private set; }
        [field: SerializeField] public Sprite Image { get; private set; }
        [field: SerializeField] public float Size { get; private set; } = 0.5f;
#if UNITY_EDITOR
        [field: ShowIf(nameof(HaveHealth))]
#endif
        [field: SerializeField] public Color HealthColor { get; private set; }
#if UNITY_EDITOR
        [field: ShowIf(nameof(HaveHealth))]
#endif
        [field: SerializeField] public Sprite HealthImage { get; private set; }

#if UNITY_EDITOR
        private bool HaveHealth() => GetComponent<Health>() != null;
#endif
    }
}