using System.Collections;
using UnityEngine;

namespace S1LV3Rman.RockFall.CoreGameplay
{
    public class LaserWeapon : BaseWeapon
    {
        [SerializeField] private float _damageInterval = 0.1f;
        [SerializeField] private int _damage = 1;
        [SerializeField] private LaserBeam _beamPrefab;
        [SerializeField] private AudioSource _fireSound;

        private bool _isFiring = false;
        private LaserBeam _currentLaserBeam;

        public override DamageType Type => DamageType.Laser;
        public override float MaxFireDistance => _beamPrefab.MaxLength;
        public override float ProjectileSpeed => float.MaxValue;

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
                    // Нанести повреждение объекту, в который попал лазер, если возможно
                    var target = _currentLaserBeam.HittedObject.GetComponentInParent<IDamageable>();
                    if (target != null)
                    {
                        var context = new DamageContext(
                            Source, this, _currentLaserBeam.EndPoint, Damage, Type, teamId: TeamId);
                        target.Receive(context);
                    }
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