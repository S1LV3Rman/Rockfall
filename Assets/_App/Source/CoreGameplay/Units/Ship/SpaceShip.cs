using UnityEngine;

namespace S1LV3Rman.RockFall.CoreGameplay
{
    public class SpaceShip : AliveTrackedMonoBehaviour, IReusableInPool
    {
        [field: SerializeField] public ShipEngines Engines { get; }
        [field: SerializeField] public ShipSteering Steering { get; }
        [field: SerializeField] public ShipWeaponry Weaponry { get; }
        public void PrepareForPulling()
        {
            gameObject.SetActive(true);
        }

        public void PrepareForReleasing()
        {
            gameObject.SetActive(false);
        }
    }
}
