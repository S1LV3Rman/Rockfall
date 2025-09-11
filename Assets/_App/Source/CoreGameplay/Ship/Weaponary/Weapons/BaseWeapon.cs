using UnityEngine;

namespace S1LV3Rman.RockFall.CoreGameplay
{
    public abstract class BaseWeapon : MonoBehaviour
    {
        [field: SerializeField] public AimPoint AimPoint { get; private set; }
        protected IInstigator Owner { get; private set; }
        public float DistanceToAimTarget { get; set; }
        public abstract DamageType DamageType { get; }
        public abstract float MaxFireDistance { get; }
        public abstract float ProjectileSpeed { get; }

        protected virtual void Update() => 
            AimPoint.transform.localPosition = Vector3.forward * DistanceToAimTarget;

        public void SetOwner(IInstigator owner) => Owner = owner;
        public abstract void SetStats(WeaponData weaponData);
        public abstract void StartFiring();
        public abstract void StopFiring();
    }
}