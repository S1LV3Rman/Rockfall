using System;
using UnityEngine;

namespace S1LV3Rman.RockFall.CoreGameplay
{
    [CreateAssetMenu(fileName = nameof(WeaponsConfig), menuName = "Config/" + nameof(WeaponsConfig), order = 0)]
    public class WeaponsConfig : ScriptableObject
    {
        [SerializeReference] private WeaponPreset[] _weapons;

        public bool TryGetWeapon(string weaponType, out WeaponPreset weaponPreset)
        {
            var weaponIndex = Array.FindIndex(_weapons, weapon => weapon.WeaponType == weaponType);
            if (weaponIndex < 0)
            {
                weaponPreset = null;
                return false;
            }

            weaponPreset = _weapons[weaponIndex];
            return true;
        } 
    }
}