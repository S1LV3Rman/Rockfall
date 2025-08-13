using UnityEngine;

namespace S1LV3Rman.RockFall.CoreGameplay
{
    public class SpaceShip : MonoBehaviour
    {
        public ShipEngines Engines;
        public ShipWeaponry Weaponry;

        public void Destroy()
        {
            Destroy(gameObject);
        }
    }
}