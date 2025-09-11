using UnityEngine;

namespace S1LV3Rman.RockFall.CoreGameplay
{
    public class DamageOnCollide : MonoBehaviour, IDamageDealer
    {
        [field: SerializeField] public IInstigator Source { get; protected set; }
        [field: SerializeField] public int BaseDamage { get; protected set; }
        [field: SerializeField] public DamageType DamageType { get; protected set; }

        [SerializeField] private bool _selfDestroy;
        
        public DamageModifier Modifier { get; }

        public void SetupDamage(IInstigator source, int damage, DamageType damageType)
        {
            Source = source;
            BaseDamage = damage;
            DamageType = damageType;
        }

        private void TryHit(GameObject other, Vector3 at)
        {
            var hitbox = other.GetComponent<DamageableHitbox>();
            if (hitbox == null)
                return;

            var damageContext = new DamageContext(Source, this, hitbox.Owner, at,
                BaseDamage, DamageType, teamId: Source.TeamId);
            hitbox.Owner.ReceiveDamage(damageContext);

            if (_selfDestroy)
                Destroy(gameObject);
        }

        private void OnTriggerEnter(Collider other) =>
            TryHit(other.gameObject, other.ClosestPoint(transform.position));

        private void OnCollisionEnter(Collision c) =>
            TryHit(c.gameObject, c.GetContact(0).point);
    }
}