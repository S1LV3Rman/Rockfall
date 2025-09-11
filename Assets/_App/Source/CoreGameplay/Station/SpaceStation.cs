using UnityEngine;

namespace S1LV3Rman.RockFall.CoreGameplay
{
    public class SpaceStation : AliveTrackedMonoBehaviour, IDamageableProvider, IReusableInPool
    {
        [field: SerializeField] public IDamageable Health { get; private set; }
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