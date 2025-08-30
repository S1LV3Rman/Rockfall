using UnityEngine;

namespace S1LV3Rman.RockFall.CoreGameplay
{
    public class Asteroid : AliveTrackedMonoBehaviour, IReusableInPool
    {
        [SerializeField] private Rigidbody _rigidbody;
        
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public Color IndicatorColor { get; private set; }
        [field: SerializeField] public Color IndicatorHealthColor { get; private set; }
        [field: SerializeField] public float IndicatorSize { get; private set; } = 0.5f;

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