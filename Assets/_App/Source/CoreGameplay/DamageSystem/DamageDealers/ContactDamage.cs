using System;
using R3;
using UnityEngine;

namespace S1LV3Rman.RockFall.CoreGameplay
{
    public class ContactDamage : MonoBehaviour, IDamageDealer
    {
        [SerializeField] private CollisionDetector _collisionDetector;
        
        [SerializeField] private DamageType _damageType;
        [SerializeField] private int _baseDamage;
        
        public Guid Id { get; } = Guid.NewGuid();
        public IDamageSource Source { get; set; }
        public DamageType DamageType => _damageType;
        public int BaseDamage => _baseDamage;
        
        private Subject<DamageContext> _onDealDamage;
        public Observable<DamageContext> OnDealDamage => _onDealDamage;
        
        private IDisposable _subscription;

        private void Start()
        {
            _subscription = _collisionDetector.OnCollision.Subscribe(TryHit);
        }

        private void OnDestroy()
        {
            _subscription.Dispose();
            _subscription = null;
        }

        public void Setup(DamageType damageType, int baseDamage)
        {
            _damageType = damageType;
            _baseDamage = baseDamage;
        }

        private void TryHit(Hit hit)
        {
            var hitBox = hit.Target.GetComponent<HitBox>();
            if (hitBox == null)
                return;

            _onDealDamage.OnNext(new DamageContext(Source, this, hitBox.Owner, hit.Point,
                BaseDamage, DamageType, teamId: Source.TeamId));
        }
    }
}