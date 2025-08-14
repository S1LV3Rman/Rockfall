using UnityEngine;

namespace S1LV3Rman.RockFall.CoreGameplay
{
    [CreateAssetMenu(fileName = nameof(AsteroidsConfig), menuName = "Config/" + nameof(AsteroidsConfig), order = 0)]
    public sealed class AsteroidsConfig : ScriptableObject
    {
        [field: SerializeField] public Asteroid BasicAsteroid { get; private set; }
    }
}