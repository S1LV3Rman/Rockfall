using R3;
using UnityEngine;

namespace S1LV3Rman.RockFall.CoreGameplay
{
    public class Health : MonoBehaviour, IDamageable
    {
        [SerializeField] private GameObject _destructionPrefab;

        public string Name => gameObject.name;
        public DamageModifier Modifier { get; private set; }

        private ReactiveProperty<int> _current;
        public ReadOnlyReactiveProperty<int> CurrentHealth => _current;
        public int MaxHealth { get; private set; }

        private Subject<DamageContext> _onReceivingDamage;
        public Observable<DamageContext> OnReceivingDamage => _onReceivingDamage;

        private Subject<DamageEvent> _onDamaged;
        public Observable<DamageEvent> OnDamaged => _onDamaged;

        public void Init(int maxHealth) => Init(maxHealth, maxHealth);

        public void Init(int maxHealth, int currentHealth)
        {
            Modifier = new DamageModifier();
            MaxHealth = maxHealth;
            _current = new ReactiveProperty<int>(currentHealth);
            _onDamaged = new Subject<DamageEvent>();
            _onReceivingDamage = new Subject<DamageContext>();
        }

        public void Deinit()
        {
            Modifier.Dispose();
            Modifier = null;

            _current.OnCompleted();
            _current.Dispose();
            _current = null;

            _onDamaged.OnCompleted();
            _onDamaged.Dispose();
            _onDamaged = null;

            _onReceivingDamage.OnCompleted();
            _onReceivingDamage.Dispose();
            _onReceivingDamage = null;
        }

        public void SetMax(int maxHealth, CurrentHealthChange currentDependence = CurrentHealthChange.SamePercent)
        {
            switch (currentDependence)
            {
                case CurrentHealthChange.Clamp:
                    MaxHealth = maxHealth;
                    if (_current.Value > MaxHealth)
                        _current.Value = maxHealth;
                    break;
                case CurrentHealthChange.SamePercent:
                    var healthPercent = (float) _current.Value / MaxHealth;
                    MaxHealth = maxHealth;
                    _current.Value = Mathf.RoundToInt(maxHealth * healthPercent);
                    break;
                case CurrentHealthChange.EqualMax:
                    MaxHealth = maxHealth;
                    _current.Value = maxHealth;
                    break;
                default:
                    MaxHealth = maxHealth;
                    break;
            }
        }

        public void ReceiveDamage(DamageContext context) =>
            _onReceivingDamage.OnNext(context);

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