using UnityEngine;

namespace Scripts
{
    public class ShipTarget : MonoBehaviour
    {
        // Спрайт для использования в качестве прицельной сетки.
        public Sprite targetImage;

        void Start()
        {
            // Получаем цвет из hex кода
            if (!ColorUtility.TryParseHtmlString("#00D8FF", out var targetColor))
                targetColor = Color.cyan;

            // Зарегистрировать новый индикатор, соответствующий
            // данному объекту, использовать полученый цвет и
            // нестандартный спрайт.
            IndicatorManager.Instance.AddIndicator(transform, targetColor, targetImage);
        }
    }
}