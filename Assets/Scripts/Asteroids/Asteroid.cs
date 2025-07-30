using UnityEngine;

namespace Scripts
{
    public class Asteroid : MonoBehaviour
    {
        // Скорость перемещения астероида.
        public float speed = 10.0f;
        [SerializeField] private Color _indicatorColor;
        [SerializeField] private Color _indicatorHealthColor;

        public void SetTarget(Transform target)
        {
            // Направить на цель
            transform.LookAt(target);

            // установить скорость перемещения твердого тела
            GetComponent<Rigidbody>().linearVelocity = transform.forward * speed;

            // Создать красный индикатор для данного астероида
            IndicatorManager.Instance.AddIndicator(transform, _indicatorColor)
                .WithHealth(_indicatorHealthColor)
                .WithDistance(target);
        }
    }
}