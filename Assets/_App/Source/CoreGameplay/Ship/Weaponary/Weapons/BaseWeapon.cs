using UnityEngine;

namespace S1LV3Rman.RockFall.CoreGameplay
{
    public abstract class BaseWeapon : MonoBehaviour
    {
        [field: SerializeField] public AimPoint AimPoint { get; private set; }
        public abstract float MaxFireDistance { get; }
        public abstract float ProjectileSpeed { get; }
        public float DistanceToAimTarget { get; set; }

        protected virtual void Update()
        {
            AimPoint.transform.localPosition = Vector3.forward * DistanceToAimTarget;
        }

        public abstract void StartFiring();

        public abstract void StopFiring();
    }
}