using UnityEngine;

namespace S1LV3Rman.RockFall.CoreGameplay
{
    public class Asteroid : AliveTrackedMonoBehaviour, IInstigator, IDamageableProvider, IReusableInPool
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private DamageOnCollide _damageOnCollide;
        [field: SerializeField] public int TeamId { get; private set; }
        [field: SerializeField] public IDamageable Health { get; private set; }

        public void SetStats(AsteroidData asteroidData)
        {
            Health.SetMax(asteroidData.Health, CurrentHealthChange.EqualMax);
            _damageOnCollide.SetupDamage(this, asteroidData.Damage, DamageType.Kinetic);
            transform.localScale *= asteroidData.Size;
        }

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