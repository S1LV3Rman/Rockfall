using System.Collections.Generic;
using UnityEngine;

namespace S1LV3Rman.RockFall
{
    public interface IRandomSource
    {
        public int IntExcluded(int min, int max);

        public int IntIncluded(int min, int max);

        public float Float();

        public float Float(float max);

        public float Float(float min, float max);

        public T Item<T>(T[] items);

        public T Item<T>(List<T> items);

        /// <summary>
        /// Generates a random normalized Vector3 with a uniform distribution.
        /// </summary>
        public Vector3 Direction();

        /// <summary>
        /// Generates a random normalized Vector3 flatten by specified axis with a uniform distribution.
        /// </summary>
        public Vector3 DirectionFlat(RandomService.Axis flattenAxis);

        public Vector3 PositionInSphere(Vector3 center, float radius);
        public Vector3 PositionInSphereFlat(Vector3 center, float radius, RandomService.Axis flattenAxis);
        public Vector3 PositionInBox(Vector3 center, Vector3 size);
    }
}