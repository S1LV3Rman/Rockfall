using UnityEngine;

namespace S1LV3Rman.RockFall.CoreGameplay
{
    [RequireComponent(typeof(LineRenderer))]
    public class LaserBeam : MonoBehaviour
    {
        [SerializeField] private float noise = 1.0f;
        [SerializeField] private ParticleSystem endEffect;
        [SerializeField] private LineRenderer lineRenderer;

        private bool _isHittingAnything;
        private Vector3 _endPoint;
        private int _pointsCount;

        public void SetEndPoint(Vector3 endPoint, bool isHittingAnything = false)
        {
            _endPoint = endPoint;
            _isHittingAnything = isHittingAnything;
        }

        private void Update()
        {
            RenderLaser();
            if (endEffect != null)
                UpdateEndEffect();
        }

        private void RenderLaser()
        {
            UpdatePointsCount();

            lineRenderer.SetPosition(0, transform.position);
            for (var i = 1; i < _pointsCount - 1; i++)
            {
                var circle = Random.insideUnitCircle * noise;
                var noiseOffset = transform.right * circle.x + transform.up * circle.y;
                var position = transform.position + i * transform.forward + noiseOffset;

                lineRenderer.SetPosition(i, position);
            }

            lineRenderer.SetPosition(_pointsCount - 1, _endPoint);
        }

        private void UpdatePointsCount()
        {
            var length = Vector3.Distance(transform.position, _endPoint);
            _pointsCount = Mathf.CeilToInt(length) + 1;
            lineRenderer.positionCount = _pointsCount;
        }

        private void UpdateEndEffect()
        {
            if (_isHittingAnything)
            {
                endEffect.transform.position = _endPoint;
                if (!endEffect.isPlaying)
                    endEffect.Play();
            }
            else if (endEffect.isPlaying)
            {
                endEffect.Stop();
            }
        }
    }
}