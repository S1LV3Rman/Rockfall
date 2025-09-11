using System;
using R3;
using UnityEngine;
using UnityEngine.EventSystems;

namespace S1LV3Rman.RockFall
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class UIPanel : UIBehaviour
    {
        [OnFieldChanged(nameof(SetOpened))]
        [SerializeField] private bool _isOpened;

        private ReactiveProperty<bool> _isOpenedInner;
        public ReadOnlyReactiveProperty<bool> IsOpened => _isOpenedInner;

        private IDisposable _subscription;

        protected override void Awake()
        {
            _isOpenedInner = new ReactiveProperty<bool>(_isOpened);
            _subscription = _isOpenedInner.Subscribe(OnOpenedChanged);
        }

        public void SetOpened(bool isOpened) => _isOpenedInner.Value = isOpened;
        public void Open() => SetOpened(true);
        public void Close() => SetOpened(false);
        public void Toggle() => SetOpened(!IsOpened.CurrentValue);

        private void OnOpenedChanged(bool isOpened)
        {
            if (isOpened)
                OnOpen();
            else
                OnClose();
        }
        protected abstract void OnOpen();

        protected abstract void OnClose();

        protected override void OnDestroy() => _subscription.Dispose();
    }
}