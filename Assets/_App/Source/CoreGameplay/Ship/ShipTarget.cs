using UnityEngine;

namespace S1LV3Rman.RockFall
{
    public class ShipTarget : MonoBehaviour
    {
        [SerializeField] private Color _color;
        [SerializeField] private Sprite _targetImage;
        [SerializeField] private float _size = 0.75f;

        private void Start()
        {
            IndicatorManager.Instance.AddIndicator(transform, _color, _size, _targetImage);
        }
    }
}