using System;
using R3;
using UnityEngine;

namespace S1LV3Rman.RockFall.CoreGameplay
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private CollisionDetector _collisionDetector;
        
        public Observable<Hit> OnHit => _collisionDetector.OnCollision;

        private void Start()
        {
            OnHit.Subscribe(SelfDestroy).RegisterTo(destroyCancellationToken);
        }

        public void Launch(float speed, float lifetime)
        {
            _rigidbody.linearVelocity = transform.forward * speed;
            Destroy(gameObject, lifetime);
        }

        private void SelfDestroy(Hit _) => Destroy(gameObject);
    }
}