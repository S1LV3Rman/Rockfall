using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts
{
    public class ShipWeaponry : MonoBehaviour
    {
        [SerializeField] private List<Transform> _weaponSlots;
        [SerializeField] private LaserWeapon _laserWeaponPrefab;
        [SerializeField] private RapidWeapon _rapidWeaponPrefab;

        public void EquipWeapons(WeaponType weaponType)
        {
            foreach (var weaponSlot in _weaponSlots)
            {
                switch (weaponType)
                {
                    case WeaponType.Undefined:
                        break;
                    case WeaponType.RapidFire:
                        Instantiate(_rapidWeaponPrefab, weaponSlot);
                        break;
                    case WeaponType.LaserBeam:
                        Instantiate(_laserWeaponPrefab, weaponSlot);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(weaponType), weaponType, null);
                }
            }
        }
    }
}