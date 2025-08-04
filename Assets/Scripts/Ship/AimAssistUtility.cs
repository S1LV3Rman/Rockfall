using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts
{
    public static class AimAssistUtility
    {
        public static bool TryGetLeadPoint(
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

        public static Transform FindBestTargetInCone(
            Vector3 aimOrigin,
            Vector3 direction,
            IEnumerable<Transform> targets,
            float coneAngleDegrees,
            float maxDistance)
        {
            var cosThreshold = Mathf.Cos(coneAngleDegrees * Mathf.Deg2Rad);
            Transform bestTarget = null;
            var bestDot = -1f; // closer to 1 is better

            foreach (var asteroid in targets)
            {
                var toAsteroid = asteroid.position - aimOrigin;
                var distance = toAsteroid.magnitude;
                if (distance > maxDistance)
                    continue;

                var toAsteroidDir = toAsteroid / distance; // normalize
                var dot = Vector3.Dot(direction, toAsteroidDir);

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