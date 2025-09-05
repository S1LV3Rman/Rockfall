using UnityEngine;

namespace S1LV3Rman.RockFall.CoreGameplay
{
    public class DamageOnCollide : MonoBehaviour, IDamageDealer
    {
        [field: SerializeField] public IInstigator Source { get; set; }
        [field: SerializeField] public int Damage { get; set; }
        [field: SerializeField] public DamageType Type { get; set; }
        [field: SerializeField] public int TeamId { get; set; }
        [SerializeField] private bool _selfDestroy;

        private void TryHit(GameObject other, Vector3 at)
        {
            // Prefer hitbox → owner
            var hitbox = other.GetComponent<DamageableHitbox>();
            if (hitbox == null)
                return;

            var damageContext = new DamageContext(Source, this, at, Damage, Type, teamId: TeamId);
            hitbox.Owner.Receive(damageContext);

            if (_selfDestroy)
                Destroy(gameObject);
        }

        private void OnTriggerEnter(Collider other) =>
            TryHit(other.gameObject, other.ClosestPoint(transform.position));

        private void OnCollisionEnter(Collision c) =>
            TryHit(c.gameObject, c.GetContact(0).point);
    }
}