using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace S1LV3Rman.RockFall.CoreGameplay
{
    public sealed class AimAssistance : MonoBehaviour
    {
        [SerializeField] private float _aimAssistCone = 15f;
        [SerializeField] private float _aimAssistStrength = 0.5f;

        [SerializeField] private float _angleScoreImpact = 0.8f;
        [SerializeField] private float _distanceScoreImpact = 0.2f;
        [SerializeField] private float _switchTargetThreshold = 0.15f;

        [SerializeField] private Transform _currentTarget;

        private IWeaponized _controlledWeaponized;

        private void Aim(ICollection<Transform> targets)
        {
            var frameAssistStrength = 1f - Mathf.Exp(-_aimAssistStrength * Time.deltaTime);
            var maxFireDistance = _controlledWeaponized.Weapons.Max(weapon => weapon.MaxFireDistance);
            _currentTarget = FindBestTargetInCone(targets, maxFireDistance);

            foreach (var weapon in _controlledWeaponized.Weapons)
            {
                Vector3 targetAimPoint;
                if (_currentTarget != null)
                {
                    var targetBody = _currentTarget.GetComponent<Rigidbody>();
                    if (targetBody != null)
                    {
                        TryGetLeadPoint(
                            weapon.Position,
                            targetBody.position,
                            targetBody.linearVelocity,
                            weapon.ProjectileSpeed,
                            out targetAimPoint);
                    }
                    else
                    {
                        targetAimPoint = _currentTarget.position;
                    }
                }
                else
                {
                    targetAimPoint = transform.forward * weapon.MaxFireDistance;
                }

                var desiredAimDirection = targetAimPoint - weapon.Position;
                var currentAim = weapon.Direction;
                var targetAim = Vector3.Slerp(currentAim, desiredAimDirection.normalized, frameAssistStrength);
                weapon.Direction = targetAim;
            }
        }

        private Transform FindBestTargetInCone(
            ICollection<Transform> targets,
            float maxDistance)
        {
            var dotThreshold = Mathf.Cos(_aimAssistCone * Mathf.Deg2Rad);
            
            Transform bestTarget = null;
            var bestScore = 0f;

            var currentTargetScore = GetTargetAimScore(_currentTarget, dotThreshold, maxDistance);
            if (currentTargetScore > 0f)
            {
                bestTarget = _currentTarget;
                bestScore = currentTargetScore;
            }

            foreach (var target in targets)
            {
                var targetScore = GetTargetAimScore(target, dotThreshold, maxDistance);
                if (targetScore <= bestScore)
                    continue;
                
                if (_currentTarget == null || targetScore < currentTargetScore + _switchTargetThreshold)
                    continue;

                bestTarget = target;
                bestScore = targetScore;
            }

            return bestTarget;
        }

        private float GetTargetAimScore(Transform target, float dotThreshold, float maxDistance)
        {
            if (_currentTarget == null)
                return 0f;
            
            var toTarget = target.position - transform.position;
            var distance = toTarget.magnitude;
            if (distance > maxDistance)
                return 0f;

            var dot = Vector3.Dot(transform.forward, toTarget.normalized);
            if (dot > dotThreshold)
                return 0f;

            var distanceScore = Mathf.Pow(1f - distance / maxDistance, 2f);
            return dot * _angleScoreImpact 
                   + distanceScore * _distanceScoreImpact;
        }

        private static bool TryGetLeadPoint(
            Vector3 shooterPos,
            Vector3 targetPos,
            Vector3 targetVelocity,
            float projectileSpeed,
            out Vector3 aimPoint)
        {
            aimPoint = targetPos;

            // Handle very high speed as instant hit
            if (projectileSpeed >= 1e9f || float.IsInfinity(projectileSpeed))
                return true;

            Vector3 displacement = targetPos - shooterPos;
            float distanceSqr = displacement.sqrMagnitude;

            float speedSqr = projectileSpeed * projectileSpeed;

            // Also catches overflow here
            if (float.IsInfinity(speedSqr))
                return true;

            float targetSpeedSqr = targetVelocity.sqrMagnitude;

            float a = targetSpeedSqr - speedSqr;
            float b = 2f * Vector3.Dot(targetVelocity, displacement);
            float c = distanceSqr;

            if (Mathf.Abs(a) < 0.0001f)
            {
                float t0 = -c / b;
                if (t0 < 0f) return false;

                aimPoint = targetPos + targetVelocity * t0;
                return true;
            }

            float discriminant = b * b - 4f * a * c;

            if (discriminant is < 0f or float.NaN)
                return false;

            float sqrtDisc = Mathf.Sqrt(discriminant);
            float t1 = (-b + sqrtDisc) / (2f * a);
            float t2 = (-b - sqrtDisc) / (2f * a);

            float t = Mathf.Min(t1, t2);
            if (t < 0f)
                t = Mathf.Max(t1, t2);

            if (t < 0f)
                return false;

            aimPoint = targetPos + targetVelocity * t;
            return true;
        }
    }
}