using R3;
using S1LV3Rman.RockFall.CoreGameplay;
using UnityEngine;

namespace S1LV3Rman.RockFall
{
    public sealed class CollisionDetector : MonoBehaviour
    {
        private readonly Subject<Hit> _onCollision = new();
        public Observable<Hit> OnCollision => _onCollision;

        private void OnTriggerEnter(Collider other) =>
            Collision(other.gameObject, other.ClosestPoint(transform.position));

        private void OnCollisionEnter(Collision c) =>
            Collision(c.gameObject, c.GetContact(0).point);

        private void Collision(GameObject target, Vector3 point) =>
            _onCollision.OnNext(new Hit
            {
                Target = target,
                Point = point,
            });
    }
}