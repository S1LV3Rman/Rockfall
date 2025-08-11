using UnityEngine;
using UnityEngine.UI;

namespace S1LV3Rman.RockFall
{
    [RequireComponent(typeof(Image))]
    public class ImageBlinkingAnimation : MonoBehaviour
    {
        [SerializeField] private float _frequency = 1f;
        [SerializeField] private Image _image;
        
        private Color _currentColor;
        private bool _isFading;
        
        private void Awake()
        {
            if (_image == null)
                _image = GetComponent<Image>();
        }
    
        void OnEnable()
        {
            _isFading = true;
            
            _currentColor = _image.color;
            _currentColor.a = 1f;
            _image.color = _currentColor;
        }

        void Update()
        {
            if (_isFading)
            {
                _currentColor.a -= _frequency * Time.deltaTime;
                if (_currentColor.a <= 0f)
                {
                    _currentColor.a = -_currentColor.a;
                    _isFading = false;
                }
            }
            else
            {
                _currentColor.a += _frequency * Time.deltaTime;
                if (_currentColor.a >= 1f)
                {
                    _currentColor.a = 2 - _currentColor.a;
                    _isFading = true;
                }
            }
        
            _image.color = _currentColor;
        }
    }
}
