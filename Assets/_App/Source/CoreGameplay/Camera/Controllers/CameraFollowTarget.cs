using UnityEngine;
#if UNITY_EDITOR
#endif

namespace S1LV3Rman.RockFall
{
    public sealed class CameraFollowTarget : CameraController
    {
        [OnFieldChanged(nameof(SetTarget))]
        [SerializeField] private Transform _target;
        [SerializeField] [HideInInspector] private Transform _currentTarget;
        
        public Transform CurrentTarget
        {
            get => _currentTarget;
            private set
            {
                _currentTarget = value;
                _target = value;
                _virtualCamera.Follow = value;
            }
        }

        public void SetTarget(Transform target)
        {
            CurrentTarget = target;
        }

        public override void SetActive(bool isActive)
        {
            if (isActive) 
                SetTarget(_target);
        }
    }
}