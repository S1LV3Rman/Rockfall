using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Scripts
{
    public class DamageNumber : UIBehaviour
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private AnimationCurve _flyOverLifetime;
        [SerializeField] private float _horizontalDeviation;
        [SerializeField] private AnimationCurve _lifetimePerAmount;
        [SerializeField] private Gradient _colorPerAmount;
        [SerializeField] private AnimationCurve _sizePerAmount;
        [SerializeField] private float _deathSizeDecrease;

        private RectTransform _rectTransform;
        private RectTransform _parent;
        
        private Vector3 _initialPosition;
        private Camera _camera;

        private float _deviation;
        private float _initialSize;
        private float _showTime;
        private float _deathTime;
        private bool _isShown;

        public bool IsAlive => Time.time < _deathTime;

        protected override void Awake()
        {
            _rectTransform = (RectTransform) transform;
            _parent = (RectTransform) transform.parent;
        }

        public void Show(Vector3 startPosition, int damageAmount, Camera mainCamera)
        {
            _initialPosition = startPosition;
            _camera = mainCamera;

            _deviation = Random.Range(-_horizontalDeviation, _horizontalDeviation);
            _deathTime = Time.time + _lifetimePerAmount.Evaluate(damageAmount);
            _showTime = Time.time;

            _text.SetText(damageAmount.ToString());
            _text.fontSize = _initialSize = _sizePerAmount.Evaluate(damageAmount);
            _text.color = _colorPerAmount.Evaluate(damageAmount);

            _isShown = true;
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            _isShown = false;
            gameObject.SetActive(false);
        }

        private void LateUpdate()
        {
            if (!_isShown || !IsAlive)
                return;

            var lifetime = Time.time - _showTime;
            SetPositionAt(lifetime);

            var normalizedLifetime = Mathf.InverseLerp(_showTime, _deathTime, Time.time);
            _text.fontSize = Mathf.Lerp(_initialSize, _initialSize * _deathSizeDecrease, normalizedLifetime);
        }

        public void SetPositionAt(float lifetime)
        {
            var positionDelta = new Vector2(
                _deviation * lifetime,
                _flyOverLifetime.Evaluate(lifetime));
            Vector2 startPosition = _camera.WorldToScreenPoint(_initialPosition);
            var targetPosition = startPosition + positionDelta;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _parent, targetPosition, _camera, out var localPoint);
            
            _rectTransform.anchoredPosition = localPoint;
        }
    }
}