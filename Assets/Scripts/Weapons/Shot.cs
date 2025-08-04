using UnityEngine;

namespace Scripts
{
    public class Shot : MonoBehaviour
    {
        [SerializeField] private float speed = 100.0f;
        [SerializeField] private float lifetime = 5.0f;
        [SerializeField] private DamageOnCollide _damageOnCollide;

        public int Damage
        {
            get => _damageOnCollide.Damage;
            set => _damageOnCollide.Damage = value;
        }

        public float Speed => speed;
        public float Lifetime => lifetime;

        void Start()
        {
            GetComponent<Rigidbody>().linearVelocity = transform.forward * speed;
            Destroy(gameObject, lifetime);
        }
    }
}