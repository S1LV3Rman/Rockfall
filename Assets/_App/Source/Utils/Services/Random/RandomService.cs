using System.Collections.Generic;
using UnityEngine;

namespace S1LV3Rman.RockFall
{
    public sealed class RandomService : IRandomSource
    {
        public enum Axis
        {
            X,
            Y,
            Z
        }
        
        private readonly IRandomSource _source;

        public RandomService()
        {
            _source = GetRandomSource();
        }

        public IRandomSource GetRandomSource() => new RandomSource();
        public IRandomSource GetRandomSource(int seed) => new RandomSource(seed);

        public int IntExcluded(int min, int max) => _source.IntExcluded(min, max);
        public int IntIncluded(int min, int max) => _source.IntIncluded(min, max);
        public float Float() => _source.Float();
        public float Float(float max) => _source.Float(max);
        public float Float(float min, float max) => _source.Float(min, max);
        public T Item<T>(T[] items) => _source.Item(items);
        public T Item<T>(List<T> items) => _source.Item(items);
        public Vector3 Direction() => _source.Direction();

        public Vector3 DirectionFlat(Axis flattenAxis) =>
            _source.DirectionFlat(flattenAxis);

        public Vector3 PositionInSphere(Vector3 center, float radius) =>
            _source.PositionInSphere(center, radius);

        public Vector3 PositionInSphereFlat(Vector3 center, float radius, Axis flattenAxis) =>
            _source.PositionInSphereFlat(center, radius, flattenAxis);

        public Vector3 PositionInBox(Vector3 center, Vector3 size) =>
            _source.PositionInBox(center, size);
    }
}