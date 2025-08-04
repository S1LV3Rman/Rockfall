using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts
{
    public class ShipWeaponry : MonoBehaviour
    {
        [SerializeField] private Transform _aimPoint;
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

        public Transform FindBestTargetInCone(
            Vector3 aimOrigin,
            Vector3 aimDirection,
            IEnumerable<Transform> asteroids,
            float coneAngleDegrees,
            float maxDistance)
        {
            float cosThreshold = Mathf.Cos(coneAngleDegrees * Mathf.Deg2Rad);
            Transform bestTarget = null;
            float bestDot = -1f; // closer to 1 is better

            foreach (var asteroid in asteroids)
            {
                Vector3 toAsteroid = asteroid.position - aimOrigin;
                float distance = toAsteroid.magnitude;
                if (distance > maxDistance)
                    continue;

                Vector3 toAsteroidDir = toAsteroid / distance; // normalize
                float dot = Vector3.Dot(aimDirection, toAsteroidDir);

                if (dot > cosThreshold && dot > bestDot)
                {
                    bestDot = dot;
                    bestTarget = asteroid;
                }
            }

            return bestTarget;
        }
    }
}