using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace S1LV3Rman.RockFall.CoreGameplay
{
    public class HealthIndicator : UIBehaviour, IIndicatorModification
    {
        [SerializeField] private Image _healthBar;
        
        private Health _target;

        public void SetTarget(Health target)
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
            _healthBar.fillAmount = _target.Max > 0
                ? (float) _target.Current.CurrentValue / _target.Max
                : 0f;
        }

        public void Remove()
        {
            Destroy(gameObject);
        }
    }
}