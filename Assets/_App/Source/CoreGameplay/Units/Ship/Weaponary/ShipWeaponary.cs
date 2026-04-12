using System;
using System.Collections.Generic;
using UnityEngine;

namespace S1LV3Rman.RockFall.CoreGameplay
{
    public class ShipWeaponry : MonoBehaviour, IWeaponized
    {
        [field: SerializeField] public List<Transform> WeaponSlots { get; }
        
        private readonly List<IWeapon> _equippedWeapons = new();
        public IReadOnlyCollection<IWeapon> Weapons => _equippedWeapons;
        
        public AttackModifier Modifier { get; }

        public void EquipWeapon(IWeapon weapon)
        {
            _equippedWeapons.Add(weapon);
        }
    }
}