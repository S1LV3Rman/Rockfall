using System;
using UnityEngine;

namespace S1LV3Rman.RockFall.CoreGameplay
{
    public class SpaceStation : AliveTrackedMonoBehaviour, IUnit, IReusableInPool
    {
        [field: SerializeField] public Guid Id { get; } = Guid.NewGuid();
        [field: SerializeField] public int TeamId { get; }
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