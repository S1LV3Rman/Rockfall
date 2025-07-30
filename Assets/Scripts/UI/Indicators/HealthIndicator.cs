using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Scripts
{
    public class HealthIndicator : UIBehaviour
    {
        [SerializeField] private Image _healthBar;
        
        private DamageTaking _damageTaking;

        public void SetTarget(DamageTaking target)
        {
            _damageTaking = target;
            UpdateHealth();
        }

        public void SetColor(Color color)
        {
            color.a = 0.5f;
            _healthBar.color = color;
        }

        public void SetSprite(Sprite sprite) => _healthBar.sprite = sprite;

        private void LateUpdate() => UpdateHealth();

        private void UpdateHealth()
        {
            _healthBar.fillAmount = _damageTaking.MaxHealth > 0
                ? (float) _damageTaking.CurrentHealth / _damageTaking.MaxHealth
                : 0f;
        }
    }
}