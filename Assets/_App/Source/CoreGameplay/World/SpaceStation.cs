using UnityEngine;

namespace S1LV3Rman.RockFall.CoreGameplay
{
    public class SpaceStation : MonoBehaviour
    {
        [SerializeField] private Color _indicatorColor;
        [SerializeField] private Color _indicatorHealthColor;
        [SerializeField] private float _indicatorSize = 2f;

        void Start()
        {
            IndicatorManager.Instance.AddIndicator(transform, _indicatorColor, _indicatorSize)
                .WithHealth(_indicatorHealthColor)
                .WithName("Space Station");
        }
    }
}