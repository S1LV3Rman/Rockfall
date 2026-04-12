using System;
using R3;
using UnityEngine;

namespace S1LV3Rman.RockFall.CoreGameplay
{
    public abstract class BaseWeapon<TStats> : MonoBehaviour, IWeapon where TStats : WeaponStats
    {
        public Guid Id { get; } = Guid.NewGuid();
        public abstract IDamageSource Source { get; set; }
        public abstract DamageType DamageType { get; }
        public abstract int BaseDamage { get; }
        public abstract float ProjectileSpeed { get; }
        public abstract float MaxFireDistance { get; }
        public abstract Observable<DamageContext> OnDealDamage { get; }
        public Vector3 Position => transform.position;

        public Vector3 Direction
        {
            get => transform.forward;
            set => transform.forward = value;
        }

        public abstract void SetStats(TStats weaponStats);
    }
}