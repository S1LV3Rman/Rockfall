using UnityEngine;

namespace S1LV3Rman.RockFall
{
    public abstract class BaseWeapon : MonoBehaviour
    {
        [SerializeField] private Transform _aimPoint;
        public abstract float MaxFireDistance { get; }
        public abstract float ProjectileSpeed { get; }
        public float DistanceToAimTarget { get; set; }

        public void Awake()
        {
            InputManager.Instance.AddWeapon(this);
        }

        protected virtual void Update()
        {
            _aimPoint.localPosition = Vector3.forward * DistanceToAimTarget;
        }

        public void OnDestroy()
        {
            if (InputManager.Instance != null)
                InputManager.Instance.RemoveWeapon(this);
        }

        public abstract void StartFiring();

        public abstract void StopFiring();
    }
}