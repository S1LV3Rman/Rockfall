using UnityEngine;

namespace S1LV3Rman.RockFall.CoreGameplay
{
    [CreateAssetMenu(fileName = nameof(AsteroidsConfig), menuName = "Config/" + nameof(AsteroidsConfig), order = 0)]
    public sealed class AsteroidsConfig : ScriptableObject
    {
        [field: SerializeField] public IndicatorData Indicator { get; private set; }
        [field: SerializeField] public AsteroidData BasicAsteroid { get; private set; }
    }

    public struct AsteroidData
    {
        public Asteroid Prefab;
        public int Health;
        public int Damage;
        public float Size;
        public float Speed;
    }
}