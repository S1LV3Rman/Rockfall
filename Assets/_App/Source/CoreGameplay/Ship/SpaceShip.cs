using UnityEngine;

namespace S1LV3Rman.RockFall.CoreGameplay
{
    public class SpaceShip : AliveTrackedMonoBehaviour
    {
        [field: SerializeField] public ShipEngines Engines { get; }
        [field: SerializeField] public ShipWeaponry Weaponry { get; }
    }
}