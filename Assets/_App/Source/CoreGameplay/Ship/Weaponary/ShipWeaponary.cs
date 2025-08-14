using System;
using System.Collections.Generic;
using UnityEngine;

namespace S1LV3Rman.RockFall.CoreGameplay
{
    public class ShipWeaponry : MonoBehaviour
    {
        [field: SerializeField] public List<Transform> WeaponSlots { get; }
        [SerializeField] private float _aimAssistCone = 15f;
        [SerializeField][Range(0f, 10f)] private float _aimAssistStrength = 0.5f;

        private readonly List<BaseWeapon> _equippedWeapons = new();

        public Func<List<Transform>> GetAimTargets { get; set; }

        public void EquipWeapon(BaseWeapon weapon)
        {
            _equippedWeapons.Add(weapon);
        }

        private void Update()
        {
            var frameAssistStrength = 1f - Mathf.Exp(-_aimAssistStrength * Time.deltaTime);

            foreach (var equippedWeapon in _equippedWeapons)
            {
                var currentAim = equippedWeapon.transform.forward;
                var currentPosition = equippedWeapon.transform.position;

                var target = AimAssistUtility.FindBestTargetInCone(
                    currentPosition,
                    transform.forward,
                    GetAimTargets.Invoke(),
                    _aimAssistCone,
                    equippedWeapon.MaxFireDistance);

                Vector3 targetAimPoint;
                if (target != null)
                {
                    var targetBody = target.GetComponent<Rigidbody>();
                    if (targetBody != null)
                    {
                        AimAssistUtility.TryGetLeadPoint(
                            equippedWeapon.transform.position,
                            targetBody.position,
                            targetBody.linearVelocity,
                            equippedWeapon.ProjectileSpeed,
                            out targetAimPoint);
                    }
                    else
                    {
                        targetAimPoint = target.position;
                    }
                }
                else
                {
                    targetAimPoint = transform.forward * equippedWeapon.MaxFireDistance;
                }

                var desiredAimDirection = targetAimPoint - equippedWeapon.transform.position;
                var desiredAim = desiredAimDirection.normalized;
                var newAim = Vector3.Slerp(currentAim, desiredAim, frameAssistStrength);

                equippedWeapon.transform.forward = newAim;
                equippedWeapon.DistanceToAimTarget =
                    Mathf.Lerp(equippedWeapon.DistanceToAimTarget, desiredAimDirection.magnitude, frameAssistStrength);
            }
        }
    }
}