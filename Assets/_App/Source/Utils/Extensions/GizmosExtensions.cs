using UnityEngine;

namespace S1LV3Rman.RockFall
{
    public sealed class GizmosExtensions
    {
        /// <summary>
        /// Draws a wireframe cylinder using Gizmos.
        /// </summary>
        /// <param name="position">Center of the cylinder.</param>
        /// <param name="radius">Radius of the cylinder.</param>
        /// <param name="height">Height of the cylinder.</param>
        /// <param name="rotation">Orientation of the cylinder.</param>
        /// <param name="segments">Number of segments to approximate the circle.</param>
        public static void DrawWireCylinder(Vector3 position, float radius, float height, Quaternion rotation,
            int segments = 32)
        {
            var halfHeight = height / 2f;
            var up = rotation * Vector3.up;
            var centerTop = position + up * halfHeight;
            var centerBottom = position - up * halfHeight;

            var prevTop = Vector3.zero;
            var prevBottom = Vector3.zero;

            for (var i = 0; i <= segments; i++)
            {
                var angle = 2f * Mathf.PI * i / segments;
                var x = Mathf.Cos(angle) * radius;
                var z = Mathf.Sin(angle) * radius;
                var offset = rotation * new Vector3(x, 0f, z);

                var topPoint = centerTop + offset;
                var bottomPoint = centerBottom + offset;

                if (i > 0)
                {
                    Gizmos.DrawLine(prevTop, topPoint); // Top ring
                    Gizmos.DrawLine(prevBottom, bottomPoint); // Bottom ring
                    Gizmos.DrawLine(prevTop, prevBottom); // Side vertical
                }

                prevTop = topPoint;
                prevBottom = bottomPoint;
            }

            // Last vertical line
            Gizmos.DrawLine(prevTop, prevBottom);
        }
    }
}