using System.Collections;
using R3;
using UnityEngine;

namespace S1LV3Rman.RockFall.CoreGameplay
{
    public class ProjectileWeapon : BaseWeapon<ProjectileWeaponStats> , IActiveWeapon
    {
        [SerializeField] private Cooldown _cooldown;
        
        [SerializeField] private int _baseDamage;
        [SerializeField] private DamageType _damageType;
        [SerializeField] private float _projectileSpeed;
        [SerializeField] private float _projectileLifetime;

        [SerializeField] private GameObject _muzzleFlashPrefab;
        [SerializeField] private Projectile _projectilePrefab;
        [SerializeField] private AudioSource _fireSound;

        private Coroutine _fireCoroutine;

        public override IDamageSource Source { get; set; }
        public override int BaseDamage => _baseDamage;
        public override DamageType DamageType => _damageType;

        private readonly Subject<DamageContext> _onDealDamage = new();
        public override Observable<DamageContext> OnDealDamage => _onDealDamage;

        public override void SetStats(ProjectileWeaponStats weaponStats)
        {
            _baseDamage = weaponStats.Damage;
            _damageType = weaponStats.DamageType;
            _cooldown.Duration = weaponStats.Cooldown;
            _projectileSpeed = weaponStats.ProjectileSpeed;
            _projectileLifetime = weaponStats.ProjectileLifetime;

            _muzzleFlashPrefab = weaponStats.MuzzleFlashPrefab;
            _projectilePrefab = weaponStats.ProjectilePrefab;
            _fireSound.clip = weaponStats.FireSound;
        }

        public void StartFire()
        {
            _fireCoroutine = StartCoroutine(Fire());
        }

        public void StopFire()
        {
            StopCoroutine(_fireCoroutine);
            _fireCoroutine = null;
        }
        
        private IEnumerator Fire()
        {
            while (true)
            {
                if (_cooldown.Remains <= 0f)
                {
                    _cooldown.Begin();
                    
                    var shot = Instantiate(_projectilePrefab, transform.position, transform.rotation);
                    shot.OnHit.Subscribe(DealDamageOnHit).RegisterTo(shot.destroyCancellationToken);
                    shot.Launch(_projectileSpeed, _projectileLifetime);

                    if (_fireSound != null)
                        _fireSound.Play();
                }

                yield return new WaitForFixedUpdate();
            }
        }

        private void DealDamageOnHit(Hit hit)
        {
            var hitBox = hit.Target.GetComponent<HitBox>();
            if (hitBox == null)
                return;
            
            _onDealDamage.OnNext(new DamageContext(Source, this, hitBox.Owner, hit.Point,
                BaseDamage, DamageType));
        }
    }
}