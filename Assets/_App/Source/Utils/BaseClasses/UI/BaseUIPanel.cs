using System;
using UnityEngine;

namespace S1LV3Rman.RockFall.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class BaseUIPanel : MonoBehaviour
    {
        [OnFieldChanged(nameof(SetOpened))]
        [SerializeField] private bool _isOpened;

        public event Action AfterOpen;
        public event Action AfterClose;

        public bool IsOpened
        {
            get => _isOpened;
            private set => _isOpened = value;
        }

        public void SetOpened(bool open)
        {
            if (open)
                Open();
            else
                Close();
        }

        public void Open()
        {
            if (IsOpened)
                return;

            OnOpen();
            // _openTransition.Begin();
            IsOpened = true;
            AfterOpen?.Invoke();
        }

        protected abstract void OnOpen();

        public void Close()
        {
            if (!IsOpened)
                return;

            OnClose();
            // _closeTransition.Begin();
            IsOpened = false;
            AfterClose?.Invoke();
        }

        protected abstract void OnClose();

        public void Toggle()
        {
            if (IsOpened)
                Close();
            else
                Open();
        }
    }
}