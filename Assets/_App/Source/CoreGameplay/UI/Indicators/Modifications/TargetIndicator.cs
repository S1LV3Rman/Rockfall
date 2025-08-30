using R3;
using UnityEngine;
using UnityEngine.EventSystems;

namespace S1LV3Rman.RockFall.CoreGameplay
{
    public class TargetIndicator : UIBehaviour, IIndicatorModification
    {
        [SerializeField] private int _margin = 25;

        private Indicator _indicator;
        private RectTransform _indicatorRectTransform;
        private RectTransform _parentRectTransform;

        private readonly SerialDisposable _targetLostSub = new();
        private readonly Subject<TargetIndicator> _targetLost = new();
        public Observable<TargetIndicator> TargetLost => _targetLost;
        public AliveTrackedMonoBehaviour Target { get; private set; }

        public void FollowTarget(AliveTrackedMonoBehaviour target)
        {
            Target = target;
            
            _targetLostSub.Dispose();
            _targetLostSub.Disposable = target.IsAlive
                .Where(isAlive => !isAlive)
                .Take(1)
                .Subscribe(_ =>
                {
                    Target = null;
                    _targetLost.OnNext(this);
                });
        }

        public void AttachToIndicator(Indicator indicator)
        {
            _indicator = indicator;
            _indicatorRectTransform = (RectTransform) indicator.transform;
            _parentRectTransform = (RectTransform) indicator.transform.parent;
            
            UpdatePosition();
        }

        // Обновляет положение индикатора в каждом кадре
        private void LateUpdate()
        {
            if (!isActiveAndEnabled)
                return;

            if (Target == null)
                return;
            
            UpdatePosition();
        }

        private void UpdatePosition()
        {
            //Определить экранные координаты объекта
            var viewportPoint = _indicator.RenderCamera.WorldToViewportPoint(Target.transform.position);

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
                _indicator.Size = 0.5f;
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
                _indicator.Size = Mathf.Clamp(1.0f - onViewportPoint.magnitude, 0.5f, 1.0f);
            }

            // Определить видимые координаты для индикатора
            var screenPoint = _indicator.RenderCamera.ViewportToScreenPoint(viewportPoint);

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
                _parentRectTransform,
                screenPoint,
                _indicator.RenderCamera,
                out var localPosition);

            // Обновить позицию индикатора
            _indicatorRectTransform.localPosition = localPosition;
        }

        public void Remove()
        {
            _targetLostSub.Dispose();
            _targetLost.OnCompleted(Result.Success);
            Destroy(gameObject);
        }
    }
}