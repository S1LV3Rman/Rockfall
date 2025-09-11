using UnityEngine;

namespace S1LV3Rman.RockFall.CoreGameplay
{
    public class Shot : DamageOnCollide
    {
        [SerializeField] private Rigidbody _rigidbody;

        public void SetupProjectile(float projectileSpeed, float projectileLifetime)
        {
            if (_rigidbody == null)
                _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.linearVelocity = transform.forward * projectileSpeed;
            Destroy(gameObject, projectileLifetime);
        }
    }
}