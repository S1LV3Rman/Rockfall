using UnityEngine;

namespace S1LV3Rman.RockFall
{
    public abstract class BaseWeapon : MonoBehaviour
    {
        [SerializeField] private Transform _aimPoint;
        public abstract float MaxFireDistance { get; }
        public abstract float ProjectileSpeed { get; }
        public float DistanceToAimTarget { get; set; }

        protected virtual void Update()
        {
            _aimPoint.localPosition = Vector3.forward * DistanceToAimTarget;
        }

        public abstract void StartFiring();

        public abstract void StopFiring();
    }
}