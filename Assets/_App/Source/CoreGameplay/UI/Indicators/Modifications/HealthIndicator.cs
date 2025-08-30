using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace S1LV3Rman.RockFall.CoreGameplay
{
    public class HealthIndicator : UIBehaviour, IIndicatorModification
    {
        [SerializeField] private Image _healthBar;
        
        private DamageTaking _target;

        public void SetTarget(DamageTaking target)
        {
            _target = target;
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

            if (_target == null)
                return;
            
            UpdateHealth();
        }

        private void UpdateHealth()
        {
            _healthBar.fillAmount = _target.MaxHealth > 0
                ? (float) _target.CurrentHealth / _target.MaxHealth
                : 0f;
        }

        public void Remove()
        {
            Destroy(gameObject);
        }
    }
}