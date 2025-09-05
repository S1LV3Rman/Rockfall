using UnityEngine;

namespace S1LV3Rman.RockFall.CoreGameplay
{
    public class SpaceStation : AliveTrackedMonoBehaviour, IReusableInPool
    {
        [field: SerializeField] public Health Health { get; private set; }
        [field: SerializeField] public IndicatorTarget Indicator { get; private set; }
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