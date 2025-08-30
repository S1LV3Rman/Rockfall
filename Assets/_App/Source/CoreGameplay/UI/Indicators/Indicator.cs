using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace S1LV3Rman.RockFall.CoreGameplay
{
    public class Indicator : AliveTrackedUIBehaviour, IReusableInPool
    {
        [SerializeField] private Image _image;

        private readonly List<IIndicatorModification> _modifications = new();

        private Color _defaultColor;
        private Vector3 _defaultScale;
        private Sprite _defaultSprite;

        private Vector3 _normalScale;

        public float Size
        {
            set => transform.localScale = _normalScale * value;
        }

        [field: Inject] public Camera RenderCamera { get; }

        protected override void Awake()
        {
            _defaultColor = _image.color;
            _defaultScale = transform.localScale;
            _defaultSprite = _image.sprite;
        }

        public void SetColor(Color color) => _image.color = color;

        public void SetNormalSize(float size)
        {
            _normalScale = transform.localScale * size;
            Size = 1f;
        }
        public void SetSprite(Sprite sprite) => _image.sprite = sprite;

        public void AddModification(IIndicatorModification modification)
        {
            modification.AttachToIndicator(this);
            _modifications.Add(modification);
        }

        public void PrepareForPulling()
        {
            gameObject.SetActive(true);
        }

        public void PrepareForReleasing()
        {
            gameObject.SetActive(false);

            _image.color = _defaultColor;
            transform.localScale = _defaultScale;
            _image.sprite = _defaultSprite;

            foreach (var modification in _modifications)
                modification.Remove();
            _modifications.Clear();
        }
    }
}