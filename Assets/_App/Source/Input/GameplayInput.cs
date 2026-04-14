using System;
using R3;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer.Unity;

namespace S1LV3Rman.RockFall
{
    public class GameplayInput : IInitializable, IDisposable, InputAsset.IGameplayActions
    {
        private readonly InputAsset _inputAsset;

        private readonly ReactiveProperty<float> _enginePower = new(0f);
        public ReadOnlyReactiveProperty<float> EnginePower => _enginePower;

        private readonly ReactiveProperty<bool> _isFiring = new(false);
        public ReadOnlyReactiveProperty<bool> IsFiring => _isFiring;

        private readonly ReactiveProperty<Vector2> _look = new(Vector2.zero);
        public ReadOnlyReactiveProperty<Vector2> Look => _look;

        private readonly Subject<Unit> _pauseRequested = new();
        public Observable<Unit> PauseRequested => _pauseRequested;

        public GameplayInput(InputAsset inputAsset)
        {
            _inputAsset = inputAsset;
        }

        public void Initialize()
        {
            _inputAsset.Gameplay.SetCallbacks(this);
        }

        public void OnEnginePower(InputAction.CallbackContext context)
        {
            if (!context.performed && !context.canceled)
                return;

            _enginePower.Value = context.ReadValue<float>();
        }

        public void OnFire(InputAction.CallbackContext context)
        {
            if (context.performed)
                _isFiring.Value = true;
            else if (context.canceled)
                _isFiring.Value = false;
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            if (!context.performed && !context.canceled)
                return;

            _look.Value = context.ReadValue<Vector2>();
        }

        public void OnPause(InputAction.CallbackContext context)
        {
            if (context.performed)
                _pauseRequested.OnNext(Unit.Default);
        }

        public void Dispose()
        {
            _enginePower.OnCompleted();
            _enginePower.Dispose();

            _isFiring.OnCompleted();
            _isFiring.Dispose();

            _look.OnCompleted();
            _look.Dispose();

            _pauseRequested.OnCompleted();
            _pauseRequested.Dispose();
        }
    }
}
