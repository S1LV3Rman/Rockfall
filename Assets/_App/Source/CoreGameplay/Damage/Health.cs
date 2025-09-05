using R3;
using UnityEngine;

namespace S1LV3Rman.RockFall.CoreGameplay
{
    public class Health : MonoBehaviour, IDamageable
    {
        [SerializeField] private int _maxHealth = 10;
        [SerializeField] private bool _hasInvulnerabilityOnDamage;

        [ShowIf(nameof(_hasInvulnerabilityOnDamage))]
        [SerializeField] private float _invulnerabilityDuration = 0.001f;

        [SerializeField] private GameObject _destructionPrefab;

        private ReactiveProperty<int> _current;
        public ReadOnlyReactiveProperty<int> Current => _current;
        public int Max => _maxHealth;

        private float _lastDamageTime;
        private readonly Subject<DamageEvent> _onDamaged = new();
        public Observable<DamageEvent> OnDamaged => _onDamaged;
        
        private DamageProcessor _processor;

        private void Awake()
        {
            _current = new ReactiveProperty<int>(Max);
        }

        public bool CanReceive(DamageContext ctx)
        {
            if (_hasInvulnerabilityOnDamage)
            {
                var now = Time.time;
                if (now - _lastDamageTime <= _invulnerabilityDuration)
                    return false;
            }

            return _current.Value > 0;
        }

        public void Receive(DamageContext ctx)
        {
            if (!CanReceive(ctx))
                return;

            // compute final amount
            var applied = _processor.Compute(ref ctx, ctx.BaseDamage);
            if (applied <= 0) 
                return;

            _lastDamageTime = Time.time;
            _current.Value -= applied;
            _onDamaged.OnNext(new DamageEvent(ctx, applied, _current.Value));

            if (_current.Value > 0) 
                return;
            
            if (_destructionPrefab != null)
                Instantiate(_destructionPrefab, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}