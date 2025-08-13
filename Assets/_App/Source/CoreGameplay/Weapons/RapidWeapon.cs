using UnityEngine;

namespace S1LV3Rman.RockFall.CoreGameplay
{
    public class RapidWeapon : BaseWeapon
    {
        [SerializeField] private float fireDelay = 0.25f;
        [SerializeField] private int damage = 4;
        [SerializeField] private Shot shotPrefab;
        [SerializeField] private AudioSource fireSound;
        
        private bool _isFiring = false;
        private float _nextFireTime;

        public override float ProjectileSpeed => shotPrefab.Speed;
        public override float MaxFireDistance => shotPrefab.Speed * shotPrefab.Lifetime;

        public override void StartFiring() => _isFiring = true;
        public override void StopFiring() => _isFiring = false;

        protected override void Update()
        {
            base.Update();
            
            if (!_isFiring)
                return;

            if (_nextFireTime > Time.time)
                return;
            
            Fire();
            _nextFireTime = Time.time + fireDelay;
        }

        private void Fire()
        {
            var shot = Instantiate(shotPrefab,
                transform.position,
                transform.rotation);

            shot.Damage = damage;

            if (fireSound != null) 
                fireSound.Play();
        }
    }
}
