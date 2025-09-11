using System;
using UnityEngine;

namespace S1LV3Rman.RockFall.CoreGameplay
{
    [CreateAssetMenu(fileName = nameof(WeaponsConfig), menuName = "Config/" + nameof(WeaponsConfig), order = 0)]
    public class WeaponsConfig : ScriptableObject
    {
        [field: SerializeField] public KeyedList<WeaponType, WeaponData> Weapons { get; private set; }
    }

    [Serializable]
    public class WeaponData
    {
        public BaseWeapon Prefab;
        public DamageType DamageType;
        public int Damage;
        public float FireRate;
        public float Cooldown;
        public float ProjectileSpeed;
        public float ProjectileLifetime;
        public float MaxFireDistance;

        public GameObject MuzzleFlashPrefab;
        public Shot ProjectilePrefab;
        public LaserBeam LaserPrefab;
        public AudioClip FireSound;
    }
}