using DG.Tweening;
using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR
#endif

namespace S1LV3Rman.RockFall.CoreGameplay
{
    public sealed class CameraLensShift : CameraController
    {
        [OnFieldChanged(nameof(SetImmediate))]
        [SerializeField] private Vector2 _value;

        [SerializeField] private float _transitionDuration = 0.5f;
        [SerializeField] private Ease _ease = Ease.OutSine;

        [SerializeField] [HideInInspector] private Vector2 _targetLensShift;
        [SerializeField] [HideInInspector] private Vector2 _current;
        public Vector2 Current
        {
            get => _current;
            private set
            {
                _current = value;
                _value = value;
                _virtualCamera.Lens.PhysicalProperties.LensShift = value;
            }
        }

        private float CurrentX
        {
            get => _current.x;
            set
            {
                _current.x = value;
                Current = _current;
            }
        }
        private float CurrentY
        {
            get => _current.y;
            set
            {
                _current.y = value;
                Current = _current;
            }
        }

        private Sequence _doShift;

        public void Set(Vector2 lensShift, float? durationOverride = null)
        {
            if (!IsActive || lensShift == _targetLensShift)
                return;

            _doShift?.Kill();

            _targetLensShift = lensShift;
            var duration = durationOverride ?? _transitionDuration;
            if (duration > 0f)
            {
                _doShift = DOTween.Sequence();
                _doShift.Join(DOTween.To(
                    () => _virtualCamera.Lens.PhysicalProperties.LensShift.x,
                    x => _virtualCamera.Lens.PhysicalProperties.LensShift.x = x,
                    _targetLensShift.x,
                    duration));
                _doShift.Join(DOTween.To(
                    () => _virtualCamera.Lens.PhysicalProperties.LensShift.y,
                    y => _virtualCamera.Lens.PhysicalProperties.LensShift.y = y,
                    _targetLensShift.y,
                    duration));
                _doShift.Join(DOTween.To(
                    () => CurrentX,
                    x => CurrentX = x,
                    _targetLensShift.x,
                    duration));
                _doShift.Join(DOTween.To(
                    () => CurrentY,
                    y => CurrentY = y,
                    _targetLensShift.y,
                    duration));
                _doShift.SetEase(_ease);
                _doShift.SetUpdate(true);
            }
            else
            {
                _virtualCamera.Lens.PhysicalProperties.LensShift = _targetLensShift;
                Current = _targetLensShift;
#if UNITY_EDITOR
                PrefabUtility.RecordPrefabInstancePropertyModifications(_virtualCamera);
                PrefabUtility.RecordPrefabInstancePropertyModifications(this);
#endif
            }
        }

        public void SetImmediate(Vector2 lensShift) =>
            Set(lensShift, 0f);

        public override void SetActive(bool isActive)
        {
            if (isActive) 
                SetImmediate(_value);
        }
    }
}