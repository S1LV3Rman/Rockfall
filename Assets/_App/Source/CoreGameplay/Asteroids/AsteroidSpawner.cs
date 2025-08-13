using System.Collections.Generic;
using UnityEngine;

namespace S1LV3Rman.RockFall.CoreGameplay
{
    public class AsteroidSpawner : MonoBehaviour
    {
        [SerializeField] private float _radius = 250.0f;
        [SerializeField] private float _height = 50f;
        [SerializeField] private float _spawnDelay = 5.0f;
        [SerializeField] private float _spawnDeviation = 1.0f;
        [SerializeField] private Asteroid _asteroidPrefab;

        private Transform _target;
        private bool _isActive;
        private float _nextSpawnTime;

        private readonly List<Asteroid> _existingAsteroids = new();
        public IReadOnlyList<Asteroid> ExistingAsteroids => _existingAsteroids;

        public void StartLaunchingAt(Transform target)
        {
            _isActive = true;
            _target = target;
            _nextSpawnTime = Time.time;
        }

        public void StopLaunching()
        {
            _isActive = false;
        }

        private void Update()
        {
            if (!_isActive || _nextSpawnTime > Time.time)
                return;

            _existingAsteroids.Add(CreateNewAsteroid());
            _nextSpawnTime = Time.time + _spawnDelay + Random.Range(-_spawnDeviation, _spawnDeviation);
        }

        private Asteroid CreateNewAsteroid()
        {
            var asteroidPosition = GetRandomPointOnCylinderSide(_height, _radius);
            asteroidPosition += transform.position;

            var newAsteroid = Instantiate(_asteroidPrefab, transform);
            newAsteroid.transform.position = asteroidPosition;
            newAsteroid.SetTarget(_target);

            newAsteroid.OnDestroyed += RemoveAsteroid;

            return newAsteroid;
        }

        /// <summary>
        /// Returns a random point on the side surface of a vertical cylinder.
        /// </summary>
        /// <param name="height">Total height of the cylinder.</param>
        /// <param name="radius">Radius of the cylinder.</param>
        /// <returns>A Vector3 point on the lateral surface.</returns>
        private static Vector3 GetRandomPointOnCylinderSide(float height, float radius)
        {
            // Random angle around the Y-axis
            var angle = Random.Range(0f, Mathf.PI * 2f);
            var y = Random.Range(-height / 2f, height / 2f);

            var x = Mathf.Cos(angle) * radius;
            var z = Mathf.Sin(angle) * radius;

            return new Vector3(x, y, z);
        }

        private void RemoveAsteroid(Asteroid asteroid)
        {
            asteroid.OnDestroyed -= RemoveAsteroid;
            _existingAsteroids.Remove(asteroid);
        }

        public void DestroyAllAsteroids()
        {
            foreach (var asteroid in _existingAsteroids)
            {
                asteroid.OnDestroyed -= RemoveAsteroid;
                Destroy(asteroid.gameObject);
            }

            _existingAsteroids.Clear();
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.matrix = transform.localToWorldMatrix;
            DrawWireCylinder(Vector3.zero, _radius, _height, Quaternion.identity);
        }
        
        /// <summary>
        /// Draws a wireframe cylinder using Gizmos.
        /// </summary>
        /// <param name="position">Center of the cylinder.</param>
        /// <param name="radius">Radius of the cylinder.</param>
        /// <param name="height">Height of the cylinder.</param>
        /// <param name="rotation">Orientation of the cylinder.</param>
        /// <param name="segments">Number of segments to approximate the circle.</param>
        private static void DrawWireCylinder(Vector3 position, float radius, float height, Quaternion rotation, int segments = 32)
        {
            float halfHeight = height / 2f;
            Vector3 up = rotation * Vector3.up;
            Vector3 centerTop = position + up * halfHeight;
            Vector3 centerBottom = position - up * halfHeight;

            Vector3 prevTop = Vector3.zero;
            Vector3 prevBottom = Vector3.zero;

            for (int i = 0; i <= segments; i++)
            {
                float angle = 2f * Mathf.PI * i / segments;
                float x = Mathf.Cos(angle) * radius;
                float z = Mathf.Sin(angle) * radius;
                Vector3 offset = rotation * new Vector3(x, 0f, z);

                Vector3 topPoint = centerTop + offset;
                Vector3 bottomPoint = centerBottom + offset;

                if (i > 0)
                {
                    Gizmos.DrawLine(prevTop, topPoint);       // Top ring
                    Gizmos.DrawLine(prevBottom, bottomPoint); // Bottom ring
                    Gizmos.DrawLine(prevTop, prevBottom);     // Side vertical
                }

                prevTop = topPoint;
                prevBottom = bottomPoint;
            }

            // Last vertical line
            Gizmos.DrawLine(prevTop, prevBottom);
        }
    }
}