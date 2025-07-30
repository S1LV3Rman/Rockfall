using UnityEngine;
using Random = UnityEngine.Random;

namespace Scripts
{
    [RequireComponent(typeof(LineRenderer))]
    public class LaserBeam : MonoBehaviour
    {
        [SerializeField] private float noise = 1.0f;
        [SerializeField] private float maxLength = 50.0f;
        [SerializeField] private ParticleSystem endEffect;
        [SerializeField] private LineRenderer lineRenderer;
        [SerializeField] private LayerMask layerMask = 0;

        private int _pointsCount;
        private Vector3 _endPoint;
        public bool Hitting { get; private set; } = false;
        public GameObject HittedObject { get; private set; }

        void Start()
        {
            _endPoint = transform.TransformPoint(0f, 0f, maxLength);
        }

        void FixedUpdate()
        {
            UpdateRay();
        }

        void UpdateRay()
        {
            // Raycast from the location of the cube forwards
            Hitting = Physics.Raycast(transform.position, transform.forward,
                out var hit, maxLength, layerMask, QueryTriggerInteraction.Ignore);
            
            if (Hitting)
            {
                _endPoint = hit.point;
                HittedObject = hit.collider.gameObject;
            }
            else
            {
                _endPoint = transform.TransformPoint(0f, 0f, maxLength);
            }
        }

        void Update()
        {
            RenderLaser();
            if (endEffect != null)
                UpdateEndEffect();
        }

        void RenderLaser()
        {
            UpdateLength();

            lineRenderer.SetPosition(0, transform.position);
            for (var i = 1; i < _pointsCount - 1; i++)
            {
                // Set the position here to the current location and
                // project it in the forward direction of the object it is attached to
                var circle = Random.insideUnitCircle * noise;
                var noiseOffset = transform.right * circle.x + transform.up * circle.y;
                var position = transform.position + i * transform.forward + noiseOffset;

                lineRenderer.SetPosition(i, position);
            }
            lineRenderer.SetPosition(_pointsCount - 1, _endPoint);
        }

        private void UpdateEndEffect()
        {
            if (Hitting)
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

        void UpdateLength()
        {
            var length = Hitting
                ? Vector3.Distance(transform.position, _endPoint)
                : maxLength;

            _pointsCount = Mathf.CeilToInt(length) + 1;
            lineRenderer.positionCount = _pointsCount;
        }
    }
}