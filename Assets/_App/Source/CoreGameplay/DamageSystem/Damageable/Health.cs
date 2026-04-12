using R3;
using UnityEngine;

namespace S1LV3Rman.RockFall.CoreGameplay
{
    public class Health : MonoBehaviour, IDamageable
    {
        [SerializeField] private GameObject _destructionPrefab;
        
        public IUnit Owner { get; }

        private ReactiveProperty<int> _current;
        public ReadOnlyReactiveProperty<int> CurrentHealth => _current;
        
        private ReactiveProperty<int> _maxHealth;
        public ReadOnlyReactiveProperty<int> MaxHealth => _maxHealth;

        private Subject<DamageEvent> _onDamaged;
        public Observable<DamageEvent> OnDamaged => _onDamaged;

        public void Init(int maxHealth) => Init(maxHealth, maxHealth);

        public void Init(int maxHealth, int currentHealth)
        {
            _maxHealth = new ReactiveProperty<int>(maxHealth);
            _current = new ReactiveProperty<int>(currentHealth);
            _onDamaged = new Subject<DamageEvent>();
        }

        public void Deinit()
        {
            _current.OnCompleted();
            _current.Dispose();
            _current = null;
            
            _maxHealth.OnCompleted();
            _maxHealth.Dispose();
            _maxHealth = null;

            _onDamaged.OnCompleted();
            _onDamaged.Dispose();
            _onDamaged = null;
        }

        public void SetMax(int maxHealth, CurrentHealthChange currentDependence = CurrentHealthChange.SamePercent)
        {
            switch (currentDependence)
            {
                case CurrentHealthChange.Clamp:
                    _maxHealth.Value = maxHealth;
                    if (_current.Value > _maxHealth.Value)
                        _current.Value = maxHealth;
                    break;
                case CurrentHealthChange.SamePercent:
                    var healthPercent = (float) _current.Value / _maxHealth.Value;
                    _maxHealth.Value = maxHealth;
                    _current.Value = Mathf.RoundToInt(maxHealth * healthPercent);
                    break;
                case CurrentHealthChange.EqualMax:
                    _maxHealth.Value = maxHealth;
                    _current.Value = maxHealth;
                    break;
                case CurrentHealthChange.Undefined:
                default:
                    _maxHealth.Value = maxHealth;
                    break;
            }
        }

        public void ApplyDamage(DamageContext context, int damage)
        {
            _current.Value -= damage;
            if (damage > 0)
                _onDamaged.OnNext(new DamageEvent(context, damage, _current.Value));

            if (_current.Value > 0)
                return;

            if (_destructionPrefab != null)
                Instantiate(_destructionPrefab, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}