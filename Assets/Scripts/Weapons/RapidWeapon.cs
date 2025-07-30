using UnityEngine;

namespace Scripts
{
    public class RapidWeapon : BaseWeapon
    {
        [SerializeField] private float fireDelay = 0.25f;
        [SerializeField] private int damage = 4;
        [SerializeField] private Shot shotPrefab;
        [SerializeField] private AudioSource fireSound;
        
        private bool _isFiring = false;
        private float _nextFireTime;

        public override void StartFiring() => _isFiring = true;
        public override void StopFiring() => _isFiring = false;

        private void Update()
        {
            if (!_isFiring)
                return;

            if (_nextFireTime > Time.time)
                return;
            
            Fire();
            _nextFireTime = Time.time + fireDelay;
        }

        // Вызывается при каждом выстреле
        private void Fire()
        {
            // Создать новый снаряд с ориентацией,
            // соответствующей пушке
            var shot = Instantiate(shotPrefab,
                transform.position,
                transform.rotation);

            shot.Damage = damage;

            // Если пушка имеет компонент источника звука,
            // воспроизвести звуковой эффект
            if (fireSound != null) 
                fireSound.Play();
        }
    }
}
