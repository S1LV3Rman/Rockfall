using System;
using R3;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace S1LV3Rman.RockFall.CoreGameplay
{
    public class HealthIndicator : UIBehaviour, IIndicatorModification
    {
        [SerializeField] private Image _healthBar;

        private IDamageable _damageable;
        private IDisposable _subscriptions;

        public void SetTarget(IDamageable damageable)
        {
            _damageable = damageable;

            _subscriptions = Disposable.Combine(
                damageable.CurrentHealth.Subscribe(UpdateCurrentHealth),
                damageable.MaxHealth.Subscribe(UpdateMaxHealth));
        }

        public void SetColor(Color color)
        {
            color.a = 0.5f;
            _healthBar.color = color;
        }

        public void SetSprite(Sprite sprite) => _healthBar.sprite = sprite;

        public void AttachToIndicator(Indicator indicator)
        {
            UpdateHealth(_damageable.CurrentHealth.CurrentValue, _damageable.MaxHealth.CurrentValue);
        }

        private void UpdateCurrentHealth(int currentHealth)
        {
            var maxHealth = _damageable.MaxHealth.CurrentValue;
            UpdateHealth(currentHealth, maxHealth);
        }

        private void UpdateMaxHealth(int maxHealth)
        {
            var currentHealth = _damageable.CurrentHealth.CurrentValue;
            UpdateHealth(currentHealth, maxHealth);
        }

        private void UpdateHealth(int currentHealth, int maxHealth)
        {
            _healthBar.fillAmount = maxHealth > 0
                ? (float) currentHealth / maxHealth
                : 0f;
        }

        public void Remove()
        {
            _subscriptions.Dispose();
            Destroy(gameObject);
        }
    }
}