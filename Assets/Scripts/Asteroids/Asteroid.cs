using System;
using UnityEngine;

namespace Scripts
{
    public class Asteroid : MonoBehaviour
    {
        [SerializeField] private float speed = 10.0f;
        [SerializeField] private Color _indicatorColor;
        [SerializeField] private Color _indicatorHealthColor;
        [SerializeField] private float _indicatorSize = 0.5f;
        public event Action<Asteroid> OnDestroyed;

        private void OnDestroy()
        {
            OnDestroyed?.Invoke(this);
        }

        public void SetTarget(Transform target)
        {
            transform.LookAt(target);
            GetComponent<Rigidbody>().linearVelocity = transform.forward * speed;

            IndicatorManager.Instance.AddIndicator(transform, _indicatorColor, _indicatorSize)
                .WithHealth(_indicatorHealthColor)
                .WithDistance(target)
                .WithName(nameof(Asteroid));
        }
    }
}