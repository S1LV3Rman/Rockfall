using UnityEngine;

namespace S1LV3Rman.RockFall.CoreGameplay
{
    public sealed class ShipEngines : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;

        [SerializeField] private float _maxForwardSpeed;
        [SerializeField] private float _maxBackwardSpeed;
        [SerializeField] private float _acceleration;
        [SerializeField] private float _deceleration;
        [SerializeField] private float _turnRate;

        [field: SerializeField] public bool IsOn { get; set; }
        [field: SerializeField] private float TargetSpeed { get; set; }

        public void ToggleEngines() => IsOn = !IsOn;

        private void Update()
        {
            if (IsOn)
                UpdateVelocity();
        }

        private void UpdateVelocity()
        {
            var currentVelocity = _rigidbody.linearVelocity;

            var targetDirection = transform.forward;
            var currentSpeed = currentVelocity.magnitude;

            var newDirection = Vector3.RotateTowards(
                currentVelocity.normalized,
                targetDirection,
                _turnRate * Time.deltaTime,
                0f
            );

            var targetSpeed = TargetSpeed;

            var acceleration = currentSpeed < targetSpeed
                ? _acceleration
                : _deceleration;

            var newSpeed = Mathf.MoveTowards(
                currentSpeed,
                targetSpeed,
                acceleration * Time.deltaTime
            );

            _rigidbody.linearVelocity = newDirection * newSpeed;
        }
    }
}