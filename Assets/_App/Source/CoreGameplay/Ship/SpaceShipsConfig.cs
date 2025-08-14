using UnityEngine;

namespace S1LV3Rman.RockFall.CoreGameplay
{
    [CreateAssetMenu(fileName = nameof(SpaceShipsConfig), menuName = "Config/" + nameof(SpaceShipsConfig), order = 0)]
    public class SpaceShipsConfig : ScriptableObject
    {
        [field: SerializeField] public SpaceShip BasicShip { get; private set; }
    }
}