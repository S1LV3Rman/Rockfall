using UnityEngine;

namespace Scripts
{
    public class SpaceStation : MonoBehaviour
    {
        [SerializeField] private Color _indicatorColor;
        [SerializeField] private Color _indicatorHealthColor;

        void Start()
        {
            IndicatorManager.Instance.AddIndicator(transform, _indicatorColor)
                .WithHealth(_indicatorHealthColor);
        }
    }
}