using System;
using UnityEngine;

namespace S1LV3Rman.RockFall.CoreGameplay
{
    public class LaserWeapon : BaseWeapon
    {
        [SerializeField] private float _damageInterval;
        [SerializeField] private int _baseDamage;
        [SerializeField] private float _fireDistance;

        [SerializeField] private GameObject _muzzleFlashPrefab;
        [SerializeField] private LaserBeam _beamPrefab;
        [SerializeField] private AudioSource _fireSound;

        private bool _isFiring;
        private LaserBeam _activeBeam;
        private float _nextHitTime = float.MinValue;

        public override DamageType DamageType => DamageType.Laser;
        public override float MaxFireDistance => _fireDistance;
        public override float ProjectileSpeed => float.MaxValue;

        public override void SetStats(WeaponData weaponData)
        {
            if (weaponData.DamageType != DamageType)
                throw new ArgumentException(
                    $"Stats for {weaponData.DamageType} weapon can't be set to {DamageType} weapon");

            _baseDamage = weaponData.Damage;
            _damageInterval = weaponData.Cooldown;
            _fireDistance = weaponData.MaxFireDistance;

            _muzzleFlashPrefab = weaponData.MuzzleFlashPrefab;
            _beamPrefab = weaponData.LaserPrefab;
            _fireSound.clip = weaponData.FireSound;
        }

        public override void StartFiring()
        {
            if (_activeBeam == null)
            {
                _activeBeam = Instantiate(_beamPrefab, transform);
                _activeBeam.Setup(Owner, () => _baseDamage, _fireDistance);

                if (_fireSound != null)
                    _fireSound.Play();
            }

            _isFiring = true;
        }

        public override void StopFiring()
        {
            _isFiring = false;
            Destroy(_activeBeam.gameObject);
            _activeBeam = null;
            if (_fireSound != null)
                _fireSound.Stop();
        }

        private void FixedUpdate()
        {
            if (!_isFiring || !_activeBeam.Hitting)
                return;
            
            if (_nextHitTime > Time.time)
                return;

            if (_activeBeam.TryDealDamage())
                _nextHitTime = Time.time + _damageInterval;
        }
    }
}