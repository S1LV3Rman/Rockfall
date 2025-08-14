using System;
using UnityEngine;

namespace S1LV3Rman.RockFall.CoreGameplay
{
    public class Asteroid : AliveTrackedMonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;
        
        [field: SerializeField] public Color IndicatorColor { get; }
        [field: SerializeField] public Color IndicatorHealthColor { get; }
        [field: SerializeField] public float IndicatorSize { get; } = 0.5f;

        public void SetTarget(Transform target)
        {
            IndicatorManager.Instance.AddIndicator(transform, IndicatorColor, IndicatorSize)
                .WithHealth(IndicatorHealthColor)
                .WithDistance(target)
                .WithName(nameof(Asteroid));
        }

        public void Launch(Vector3 velocity, Vector3 angularVelocity)
        {
            _rigidbody.linearVelocity = velocity;
            _rigidbody.angularVelocity = angularVelocity;
        }
    }
}