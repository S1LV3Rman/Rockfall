using System;
using R3;
using UnityEngine;
using UnityEngine.UI;

namespace S1LV3Rman.RockFall.CoreGameplay
{
    public class TargetIndicator : AliveTrackedUIBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private int _margin = 25;

        private Vector3 _initialScale;
        private Camera _mainCamera;

        private IDisposable _subscription;

        public AliveTrackedMonoBehaviour Target { get; private set; }

        public Color Color
        {
            get => _image.color;
            set => _image.color = value;
        }

        public Sprite Sprite
        {
            get => _image.sprite;
            set
            {
                _image.sprite = value;
                HasCustomSprite = true;
            }
        }

        public float Scale
        {
            get => _initialScale.x;
            set => _initialScale = new Vector3(value, value, value);
        }

        public bool HasCustomSprite { get; private set; }

        public void FollowTarget(AliveTrackedMonoBehaviour target, Camera mainCamera)
        {
            Target = target;
            _subscription = target.IsAlive
                .Where(isAlive => !isAlive)
                .Take(1)
                .Subscribe(_ => Destroy());
            
            _mainCamera = mainCamera;
            _image.gameObject.SetActive(true);
        }

        protected override void Awake()
        {
            _initialScale = transform.localScale;
            _image.gameObject.SetActive(false);
        }

        // Обновляет положение индикатора в каждом кадре
        private void LateUpdate()
        {
            if (!isActiveAndEnabled)
                return;
            
            //Определить экранные координаты объекта
            var viewportPoint =
                _mainCamera.WorldToViewportPoint(Target.transform.position);

            // Объект за границей экрана?
            if (viewportPoint.z < 0 ||
                viewportPoint.x < 0 || viewportPoint.x > 1 ||
                viewportPoint.y < 0 || viewportPoint.y > 1)
            {
                // Сдвигаем координаты в центр экрана
                // и инвертируем, если точка позади нас
                if (viewportPoint.z < 0)
                {
                    viewportPoint.x = 0.5f - viewportPoint.x;
                    viewportPoint.y = 0.5f - viewportPoint.y;
                }
                else
                {
                    viewportPoint.x -= 0.5f;
                    viewportPoint.y -= 0.5f;
                }

                // Сдвигаем точку к плоскости экрана
                viewportPoint.z = 0;

                // Определяем в какой стороне должен находиться индикатор
                viewportPoint = viewportPoint.normalized;

                // Сдвигаем точку к границе экрана
                viewportPoint.x = Mathf.Clamp(1f - Mathf.Acos(viewportPoint.x) / 1.57f, -0.5f, 0.5f) + 0.5f;
                viewportPoint.y = Mathf.Clamp(Mathf.Asin(viewportPoint.y) / 1.57f, -0.5f, 0.5f) + 0.5f;

                // Устанавливаем размер индикатора на половину от изначального
                transform.localScale = _initialScale * 0.5f;
            }
            else
            {
                // Вычисляем положение индикатора на экране
                var onViewportPoint = viewportPoint;
                onViewportPoint.z = 0f;
                onViewportPoint.x -= 0.5f;
                onViewportPoint.y -= 0.5f;

                // Вычисляем необходимый размер индикатора
                // в зависимости от растояния от центра экрана
                transform.localScale = _initialScale *
                                       Mathf.Clamp(1.0f - onViewportPoint.magnitude,
                                           0.5f, 1.0f);
            }

            // Определить видимые координаты для индикатора
            var screenPoint =
                _mainCamera.ViewportToScreenPoint(viewportPoint);

            // Ограничить краями экрана
            screenPoint.x = Mathf.Clamp(
                screenPoint.x,
                _margin,
                Screen.width - _margin);
            screenPoint.y = Mathf.Clamp(
                screenPoint.y,
                _margin,
                Screen.height - _margin);

            // Определить, где в области холста находится видимая координата
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                transform.parent.GetComponent<RectTransform>(),
                screenPoint,
                _mainCamera,
                out var localPosition);

            // Обновить позицию индикатора
            var rectTransform = GetComponent<RectTransform>();
            rectTransform.localPosition = localPosition;
        }

        public override void Dispose()
        {
            _subscription?.Dispose();
            base.Dispose();
        }
    }
}