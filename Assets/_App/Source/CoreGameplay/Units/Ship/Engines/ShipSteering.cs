using UnityEngine;

namespace S1LV3Rman.RockFall.CoreGameplay
{
    public sealed class ShipSteering : MonoBehaviour
    {
        [SerializeField] private float _turnRate = 75f;
        [SerializeField] private float _stabilizationRate = 1f;
        [SerializeField] private Transform _worldUpReference;

        private Vector2 _steeringInput;

        public void SetSteeringInput(Vector2 steeringInput)
        {
            _steeringInput = Vector2.ClampMagnitude(steeringInput, 1f);
        }

        public void StabilizeInstantly()
        {
            transform.rotation = GetStabilizedRotation(transform.rotation, GetWorldUp());
        }

        private void OnDisable()
        {
            _steeringInput = Vector2.zero;
        }

        private void Update()
        {
            var deltaTime = Time.deltaTime;
            var worldUp = GetWorldUp();

            if (_steeringInput.sqrMagnitude > 0f)
                ApplySteering(deltaTime);

            ApplyStabilization(worldUp, deltaTime);
        }

        private void ApplySteering(float deltaTime)
        {
            var steeringStep = new Vector3(-_steeringInput.y, _steeringInput.x, 0f) * (_turnRate * deltaTime);
            transform.rotation *= Quaternion.Euler(steeringStep);
        }

        private void ApplyStabilization(Vector3 worldUp, float deltaTime)
        {
            var stabilizedRotation = GetStabilizedRotation(transform.rotation, worldUp);
            var interpolation = 1f - Mathf.Exp(-_stabilizationRate * deltaTime);

            transform.rotation = Quaternion.Slerp(transform.rotation, stabilizedRotation, interpolation);
        }

        private Quaternion GetStabilizedRotation(Quaternion currentRotation, Vector3 worldUp)
        {
            if (worldUp.sqrMagnitude <= Mathf.Epsilon)
                return currentRotation;

            return Quaternion.FromToRotation(currentRotation * Vector3.up, worldUp) * currentRotation;
        }

        private Vector3 GetWorldUp()
        {
            if (_worldUpReference != null)
                return _worldUpReference.up;

            if (transform.parent != null)
                return transform.parent.up;

            return Vector3.up;
        }
    }
}
