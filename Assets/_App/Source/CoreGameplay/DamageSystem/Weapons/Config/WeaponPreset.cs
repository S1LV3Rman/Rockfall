using System;
using UnityEngine;

namespace S1LV3Rman.RockFall.CoreGameplay
{
    [Serializable]
    public abstract class WeaponPreset
    {
        public string WeaponType;
    }
    
    [Serializable]
    public abstract class WeaponPreset<TStats> : WeaponPreset where TStats : WeaponStats
    {
        public TStats Stats;
        public BaseWeapon<TStats> Prefab;
    }

    [Serializable]
    public class LaserWeaponPreset : WeaponPreset<LaserWeaponStats>
    {
    }

    [Serializable]
    public class ProjectileWeaponPreset : WeaponPreset<ProjectileWeaponStats>
    {
    }
}