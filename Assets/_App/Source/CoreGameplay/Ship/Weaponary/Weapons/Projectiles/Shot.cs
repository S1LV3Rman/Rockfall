using UnityEngine;

namespace S1LV3Rman.RockFall.CoreGameplay
{
    public class Shot : DamageOnCollide
    {
        [SerializeField] private float speed = 100.0f;
        [SerializeField] private float lifetime = 5.0f;

        public float Speed => speed;
        public float Lifetime => lifetime;

        void Start()
        {
            GetComponent<Rigidbody>().linearVelocity = transform.forward * speed;
            Destroy(gameObject, lifetime);
        }
    }
}