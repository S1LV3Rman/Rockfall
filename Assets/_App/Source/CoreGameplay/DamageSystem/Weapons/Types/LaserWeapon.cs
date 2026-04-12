using System.Collections;
using R3;
using UnityEngine;

namespace S1LV3Rman.RockFall.CoreGameplay
{
    public class LaserWeapon : BaseWeapon<LaserWeaponStats>, IActiveWeapon
    {
        [SerializeField] private LayerMask layerMask = 0;
        [SerializeField] private Cooldown _cooldown;

        [SerializeField] private DamageType _damageType;
        [SerializeField] private int _baseDamage;
        [SerializeField] private float _fireDistance;

        [SerializeField] private LaserBeam _beamPrefab;
        [SerializeField] private AudioSource _fireSound;

        private LaserBeam _activeBeam;

        private bool _isHittingAnything;
        private GameObject _hittedObject;
        private Vector3 _hitPoint;

        private Coroutine _simulationCoroutine;
        private Coroutine _damageCoroutine;

        public override IDamageSource Source { get; set; }
        public override DamageType DamageType => _damageType;
        public override int BaseDamage => _baseDamage;

        private Subject<DamageContext> _onDealDamage;
        public override Observable<DamageContext> OnDealDamage => _onDealDamage;


        public override void SetStats(LaserWeaponStats weaponStats)
        {
            _damageType = weaponStats.DamageType;
            _baseDamage = weaponStats.Damage;
            _fireDistance = weaponStats.MaxFireDistance;
            
            _cooldown.Refresh();
            _cooldown.Duration = weaponStats.Cooldown;

            _beamPrefab = weaponStats.LaserPrefab;
            _fireSound.clip = weaponStats.FireSound;
        }

        public void StartFire()
        {
            if (_activeBeam == null)
                _activeBeam = Instantiate(_beamPrefab, transform);

            if (_fireSound != null)
                _fireSound.Play();

            _simulationCoroutine = StartCoroutine(SimulateLaser());
            _damageCoroutine = StartCoroutine(DealDamage());
        }

        public void StopFire()
        {
            StopCoroutine(_simulationCoroutine);
            _simulationCoroutine = null;

            StopCoroutine(_damageCoroutine);
            _damageCoroutine = null;

            Destroy(_activeBeam.gameObject);
            _activeBeam = null;

            if (_fireSound != null)
                _fireSound.Stop();
        }

        private IEnumerator SimulateLaser()
        {
            while (true)
            {
                _isHittingAnything = Physics.Raycast(transform.position, transform.forward,
                    out var hit, _fireDistance, layerMask, QueryTriggerInteraction.Ignore);

                if (_isHittingAnything)
                {
                    _hitPoint = hit.point;
                    _hittedObject = hit.collider.gameObject;
                    _activeBeam.SetEndPoint(_hitPoint, true);
                }
                else
                {
                    _activeBeam.SetEndPoint(transform.TransformPoint(0f, 0f, _fireDistance));
                }

                yield return new WaitForFixedUpdate();
            }
        }

        private IEnumerator DealDamage()
        {
            while (true)
            {
                if (_cooldown.Remains <= 0f
                    && _isHittingAnything
                    && TryDealDamage(_hittedObject, _hitPoint))
                {
                    _cooldown.Begin();
                }

                yield return new WaitForFixedUpdate();
            }
        }

        private bool TryDealDamage(GameObject target, Vector3 hitPoint)
        {
            if (target == null)
                return false;

            var hitBox = target.GetComponent<HitBox>();
            if (hitBox == null)
                return false;

            _onDealDamage.OnNext(new DamageContext(Source, this, hitBox.Owner, hitPoint,
                BaseDamage, DamageType, teamId: Source.TeamId));

            return true;
        }
    }
}