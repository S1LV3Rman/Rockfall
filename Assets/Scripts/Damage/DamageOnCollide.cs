using UnityEngine;

namespace Scripts
{
    public class DamageOnCollide : MonoBehaviour
    {
        [SerializeField] private int damage = 1;
        [SerializeField] private bool selfDestroy = false;

        private float _lastHitTime;
        
        public int Damage
        {
            get => damage;
            set => damage = value;
        }

        private void HitObject(GameObject objectToHit)
        {
            var theirDamageTaking = objectToHit.GetComponentInParent<DamageTaking>();
            if (theirDamageTaking != null) 
                theirDamageTaking.TakeDamage(damage);

            if (selfDestroy)
            {
                Destroy(gameObject);
                return;
            }

            var damageTaking = GetComponent<DamageTaking>();
            if (damageTaking != null)
            {
                var theirDamage = objectToHit.GetComponentInParent<DamageOnCollide>();
                if (theirDamage != null) 
                    damageTaking.TakeDamage(theirDamage.Damage);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            HitObject(other.gameObject);
        }

        private void OnCollisionEnter(Collision collision)
        {
            HitObject(collision.gameObject);
        }
    }
}