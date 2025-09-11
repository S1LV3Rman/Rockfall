using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

namespace S1LV3Rman.RockFall
{
    public class RandomSource : IRandomSource
    {
        private const float ONE_THIRD = 1f / 3f;

        private readonly Random _random;

        public RandomSource()
        {
            _random = new Random();
        }

        public RandomSource(int seed)
        {
            _random = new Random(seed);
        }

        public void Skip(int amount)
        {
            for (var i = 0; i < amount; i++)
                _random.Next();
        }

        public int IntExcluded(int min, int max) =>
            _random.Next(min, max);

        public int IntIncluded(int min, int max) =>
            _random.Next(min, max + 1);

        public float Float() =>
            (float) _random.NextDouble();

        public float Float(float max) =>
            (float) _random.NextDouble() * max;

        public float Float(float min, float max) =>
            min + (float) _random.NextDouble() * (max - min);

        public T Item<T>(T[] items) =>
            items.Length > 0
                ? items[_random.Next(0, items.Length)]
                : default;

        public T Item<T>(List<T> items) =>
            items.Count > 0
                ? items[_random.Next(0, items.Count)]
                : default;

        public Vector3 Direction()
        {
            // Generate random spherical coordinates
            var theta = Float(2f * Mathf.PI); // Random azimuthal angle (0 to 2π)
            var phi = Mathf.Acos(Float(-1f, 1f)); // Random polar angle (0 to π)

            // Convert spherical coordinates to Cartesian
            var x = Mathf.Sin(phi) * Mathf.Cos(theta);
            var y = Mathf.Sin(phi) * Mathf.Sin(theta);
            var z = Mathf.Cos(phi);

            // Apply axis constraints
            return new Vector3(x, y, z);
        }

        public Vector3 DirectionFlat(RandomService.Axis flattenAxis)
        {
            // Generate a random angle for uniform distribution
            var angle = Float(2f * Mathf.PI);
            var x = Mathf.Cos(angle);
            var y = Mathf.Sin(angle);

            // Assign components based on the zeroed axis
            return flattenAxis switch
            {
                RandomService.Axis.X => new Vector3(0, x, y),
                RandomService.Axis.Y => new Vector3(x, 0, y),
                RandomService.Axis.Z => new Vector3(x, y, 0),
                _ => Vector3.zero // Shouldn't reach here
            };
        }

        public Vector3 PositionInSphere(Vector3 center, float radius)
        {
            var r = Mathf.Pow(Float(), ONE_THIRD) * radius;
            return Direction() * r + center;
        }

        public Vector3 PositionInSphereFlat(Vector3 center, float radius, RandomService.Axis flattenAxis)
        {
            var r = Mathf.Pow(Float(), ONE_THIRD) * radius;
            return DirectionFlat(flattenAxis) * r + center;
        }

        public Vector3 PositionInBox(Vector3 center, Vector3 size)
        {
            var halfWidth = size.x * 0.5f;
            var halfHeight = size.y * 0.5f;
            var halfDepth = size.z * 0.5f;

            return new Vector3(
                Float(center.x - halfWidth, center.x + halfWidth),
                Float(center.y - halfHeight, center.y + halfHeight),
                Float(center.z - halfDepth, center.z + halfDepth)
            );
        }
    }
}