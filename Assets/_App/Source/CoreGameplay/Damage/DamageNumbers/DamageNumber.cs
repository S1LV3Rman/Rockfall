using System;
using R3;
using TMPro;
using UnityEngine;

namespace S1LV3Rman.RockFall.CoreGameplay
{
    public class DamageNumber : AliveTrackedUIBehaviour, IReusableInPool
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

        private bool _isAlive;
        
        private readonly SerialDisposable _lifetimeSub = new();
        private readonly Subject<DamageNumber> _lifetimeExpired = new();

        // Emits each time the lifetime ends
        public Observable<DamageNumber> LifetimeExpiration => _lifetimeExpired;

        protected override void Awake()
        {
            _rectTransform = (RectTransform) transform;
            _parent = (RectTransform) transform.parent;
        }

        public void Show(Vector3 startPosition, int damageAmount,
            float horizontalDeviation, float lifetime, float size, Color color,
            AnimationCurve flyOverLifetime, Camera mainCamera)
        {
            _initialPosition = startPosition;
            _camera = mainCamera;
            _flyOverLifetime = flyOverLifetime;

            _deviation = horizontalDeviation;
            _showTime = Time.time;
            _deathTime = _showTime + lifetime;

            _text.SetText(damageAmount.ToString());
            _text.fontSize = _initialSize = size;
            _text.color = color;
            
            SetPositionAt(0f);

            _isAlive = true;
            gameObject.SetActive(true);

            _lifetimeSub.Disposable = Observable
                .Timer(TimeSpan.FromSeconds(lifetime))
                .Subscribe(_ =>
                {
                    _isAlive = false;
                    _lifetimeExpired.OnNext(this);
                });
        }

        private void LateUpdate()
        {
            if (!_isAlive)
                return;
            
            var lifetime = Time.time - _showTime;
            SetPositionAt(lifetime);

            var normalizedLifetime = Mathf.InverseLerp(_showTime, _deathTime, Time.time);
            _text.fontSize = Mathf.Lerp(_initialSize, _initialSize * _deathSizeDecrease, normalizedLifetime);
        }

        private void SetPositionAt(float lifetime)
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

        public void PrepareForPulling()
        {
        }

        public void PrepareForReleasing()
        {
            gameObject.SetActive(false);
        }

        public override void Dispose()
        {
            _lifetimeSub.Dispose();
            base.Dispose();
        }
    }
}