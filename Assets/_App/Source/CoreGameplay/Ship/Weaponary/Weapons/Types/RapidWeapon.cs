using System;
using UnityEngine;

namespace S1LV3Rman.RockFall.CoreGameplay
{
    public class RapidWeapon : BaseWeapon
    {
        [SerializeField] private float _cooldown;
        [SerializeField] private int _damage;
        [SerializeField] private float _projectileSpeed;
        [SerializeField] private float _projectileLifetime;

        [SerializeField] private GameObject _muzzleFlashPrefab;
        [SerializeField] private Shot _projectilePrefab;
        [SerializeField] private AudioSource _fireSound;

        private bool _isFiring = false;
        private float _nextFireTime = float.MinValue;

        public override DamageType DamageType => DamageType.Kinetic;
        public override float ProjectileSpeed => _projectileSpeed;
        public override float MaxFireDistance => _projectileSpeed * _projectileLifetime;

        public override void SetStats(WeaponData weaponData)
        {
            if (weaponData.DamageType != DamageType)
                throw new ArgumentException(
                    $"Stats for {weaponData.DamageType} weapon can't be set to {DamageType} weapon");

            _damage = weaponData.Damage;
            _cooldown = weaponData.Cooldown;
            _projectileSpeed = weaponData.ProjectileSpeed;
            _projectileLifetime = weaponData.ProjectileLifetime;

            _muzzleFlashPrefab = weaponData.MuzzleFlashPrefab;
            _projectilePrefab = weaponData.ProjectilePrefab;
            _fireSound.clip = weaponData.FireSound;
        }

        public override void StartFiring() => _isFiring = true;
        public override void StopFiring() => _isFiring = false;

        private void FixedUpdate()
        {
            if (!_isFiring)
                return;

            if (_nextFireTime > Time.time)
                return;

            Fire();
            _nextFireTime = Time.time + _cooldown;
        }
        
        private void Fire()
        {
            var shot = Instantiate(_projectilePrefab, transform.position, transform.rotation);
            shot.SetupDamage(Owner, _damage, DamageType);
            shot.SetupProjectile(_projectileSpeed, _projectileLifetime);

            if (_fireSound != null)
                _fireSound.Play();
        }
    }
}