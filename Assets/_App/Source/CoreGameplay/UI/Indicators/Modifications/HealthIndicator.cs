using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace S1LV3Rman.RockFall.CoreGameplay
{
    public class HealthIndicator : UIBehaviour, IIndicatorModification
    {
        [SerializeField] private Image _healthBar;
        
        private Health _health;

        public void SetTarget(Health health)
        {
            _health = health;
        }

        public void SetColor(Color color)
        {
            color.a = 0.5f;
            _healthBar.color = color;
        }

        public void SetSprite(Sprite sprite) => _healthBar.sprite = sprite;

        public void AttachToIndicator(Indicator indicator)
        {
            UpdateHealth();
        }

        private void LateUpdate()
        {
            if (!isActiveAndEnabled)
                return;

            if (_health == null)
                return;
            
            UpdateHealth();
        }

        private void UpdateHealth()
        {
            _healthBar.fillAmount = _health.MaxHealth > 0
                ? (float) _health.CurrentHealth.CurrentValue / _health.MaxHealth
                : 0f;
        }

        public void Remove()
        {
            Destroy(gameObject);
        }
    }
}