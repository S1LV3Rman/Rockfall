using System;
using System.Collections.Generic;
using UnityEngine;

namespace S1LV3Rman.RockFall
{
    [CreateAssetMenu(fileName = nameof(WeaponsConfig), menuName = "Config/" + nameof(WeaponsConfig), order = 0)]
    public class WeaponsConfig : ScriptableObject
    {
        public List<WeaponData> Weapons;
    }

    [Serializable]
    public struct WeaponData
    {
        public WeaponType Type;
        public BaseWeapon Prefab;
    }
}