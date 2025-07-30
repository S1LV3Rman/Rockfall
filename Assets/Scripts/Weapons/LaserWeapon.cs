using System.Collections;
using UnityEngine;

namespace Scripts
{
    public class LaserWeapon : BaseWeapon
    {
        [SerializeField] private float _damageInterval = 0.1f;
        [SerializeField] private int _damage = 1;
        [SerializeField] private LaserBeam _beamPrefab;
        [SerializeField] private AudioSource _fireSound;

        private bool _isFiring = false;
        private LaserBeam _currentLaserBeam;

        public override void StartFiring()
        {
            StartCoroutine(Firing());
        }

        public override void StopFiring()
        {
            _isFiring = false;
        }

        private IEnumerator Firing()
        {
            _isFiring = true;

            Fire();

            // Продолжать итерации, пока isFiring равна true
            while (_isFiring)
            {
                if (_currentLaserBeam.Hitting)
                {
                    // Нанести повреждение объекту, в который попал лазер,
                    // если возможно.
                    var theirDamage = _currentLaserBeam.HittedObject.GetComponentInParent<DamageTaking>();
                    if (theirDamage) 
                        theirDamage.TakeDamage(_damage);
                }

                // Ждать damageInterval секунд перед
                // следующим нанесением урона
                yield return new WaitForSeconds(_damageInterval);
            }

            Destroy(_currentLaserBeam.gameObject);
            _currentLaserBeam = null;
        }

        // Создаёт лазерные лучи
        private void Fire()
        {
            _currentLaserBeam = Instantiate(_beamPrefab, transform);

            // Если пушка имеет компонент источника звука,
            // воспроизвести звуковой эффект
            if (_fireSound != null)
                _fireSound.Play();
        }
    }
}