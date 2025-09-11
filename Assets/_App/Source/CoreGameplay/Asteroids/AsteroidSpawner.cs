using R3;
using UnityEngine;

namespace S1LV3Rman.RockFall.CoreGameplay
{
    public struct AsteroidSpawnRequest : ISpawnRequest
    {
        public readonly Vector3 Position;
        public readonly Vector3 Direction; // normalized

        public AsteroidSpawnRequest(in Vector3 position, in Vector3 direction)
        {
            Position = position;
            Direction = direction;
        }
    }

    public sealed class AsteroidSpawner : MonoBehaviour, ISpawner<AsteroidSpawnRequest>
    {
        [SerializeField] private float _radius = 250f;
        [SerializeField] private float _height = 50f;
        [SerializeField] private float _spawnDelay = 5f;
        [SerializeField] private float _spawnDeviation = 1f;

        private Transform _target;
        private bool _isActive;
        private float _nextSpawnTime;

        private readonly Subject<AsteroidSpawnRequest> _requests = new();
        public Observable<AsteroidSpawnRequest> Requests { get; }

        public void SetTarget(Transform target)
        {
            _target = target;
        }
        
        public void SetActive(bool active)
        {
            _isActive = active;
            if (active)
                _nextSpawnTime = Time.time;
        }

        private void Update()
        {
            if (!_isActive || _nextSpawnTime > Time.time)
                return;

            var spawnRequest = CreateSpawnRequest();
            _requests.OnNext(spawnRequest);

            _nextSpawnTime = Time.time + _spawnDelay + Random.Range(-_spawnDeviation, _spawnDeviation);
        }

        private AsteroidSpawnRequest CreateSpawnRequest()
        {
            var position = GetRandomPointOnCylinderSide(_height, _radius) + transform.position;
            var direction = _target != null
                ? _target.position - position
                : transform.forward;

            if (direction.sqrMagnitude < 1e-6f)
                direction = transform.forward;
            direction.Normalize();

            return new AsteroidSpawnRequest(position, direction);
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

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.matrix = transform.localToWorldMatrix;
            GizmosExtensions.DrawWireCylinder(Vector3.zero, _radius, _height, Quaternion.identity);
        }

        private void OnDestroy()
        {
            _requests.OnCompleted();
            _requests.Dispose();
        }
    }
}