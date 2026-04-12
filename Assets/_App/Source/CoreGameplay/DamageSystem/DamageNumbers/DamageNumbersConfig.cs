using UnityEngine;

namespace S1LV3Rman.RockFall.CoreGameplay
{
    [CreateAssetMenu(fileName = nameof(DamageNumbersConfig), menuName = "Config/" + nameof(DamageNumbersConfig),
        order = 0)]
    public sealed class DamageNumbersConfig : ScriptableObject
    {
        [field: SerializeField] public DamageNumber Prefab { get; private set; }
        [field: SerializeField] public AnimationCurve FlyOverLifetime { get; private set; }
        [field: SerializeField] public float HorizontalDeviation { get; private set; }
        [field: SerializeField] public AnimationCurve LifetimePerAmount { get; private set; }
        [field: SerializeField] public Gradient ColorPerAmount { get; private set; }
        [field: SerializeField] public AnimationCurve SizePerAmount { get; private set; }
        [field: SerializeField] public float DeathSizeDecrease { get; private set; }
    }
}