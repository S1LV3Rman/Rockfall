using System;
using UnityEngine;

namespace S1LV3Rman.RockFall.CoreGameplay
{
    [Serializable]
    public abstract class WeaponStats
    {
        public DamageType DamageType;
        public int Damage;
        public float FireRate;
        public float Cooldown => 1f / FireRate;
    }

    [Serializable]
    public class ProjectileWeaponStats : WeaponStats
    {
        public float ProjectileSpeed;
        public float ProjectileLifetime;
        
        public Projectile ProjectilePrefab;
        public GameObject MuzzleFlashPrefab;
        public AudioClip FireSound;
    }

    [Serializable]
    public class LaserWeaponStats : WeaponStats
    {
        public float MaxFireDistance;
        
        public LaserBeam LaserPrefab;
        public AudioClip FireSound;
    }
}