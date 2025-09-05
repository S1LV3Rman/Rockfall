using UnityEngine;

namespace S1LV3Rman.RockFall.CoreGameplay
{
    public class Asteroid : AliveTrackedMonoBehaviour, IInstigator, IReusableInPool
    {
        [SerializeField] private Rigidbody _rigidbody;
        [field: SerializeField] public Health Health { get; private set; }
        [field: SerializeField] public IndicatorTarget Indicator { get; private set; }

        public void Launch(Vector3 velocity, Vector3 angularVelocity)
        {
            _rigidbody.linearVelocity = velocity;
            _rigidbody.angularVelocity = angularVelocity;
        }

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