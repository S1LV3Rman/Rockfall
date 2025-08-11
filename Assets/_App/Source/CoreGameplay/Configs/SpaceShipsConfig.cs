using UnityEngine;

namespace S1LV3Rman.RockFall
{
    [CreateAssetMenu(fileName = nameof(SpaceShipsConfig), menuName = "Config/" + nameof(SpaceShipsConfig), order = 0)]
    public class SpaceShipsConfig : ScriptableObject
    {
        public SpaceShip BasicShip;
    }
}