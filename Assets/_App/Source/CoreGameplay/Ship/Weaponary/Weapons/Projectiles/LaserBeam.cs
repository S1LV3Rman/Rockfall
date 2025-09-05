using UnityEngine;
using Random = UnityEngine.Random;

namespace S1LV3Rman.RockFall.CoreGameplay
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
        public bool Hitting { get; private set; } = false;
        public Vector3 EndPoint { get; private set; }
        public GameObject HittedObject { get; private set; }
        public float MaxLength => maxLength;

        void Start()
        {
            EndPoint = transform.TransformPoint(0f, 0f, maxLength);
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
                EndPoint = hit.point;
                HittedObject = hit.collider.gameObject;
            }
            else
            {
                EndPoint = transform.TransformPoint(0f, 0f, maxLength);
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
            lineRenderer.SetPosition(_pointsCount - 1, EndPoint);
        }

        private void UpdateEndEffect()
        {
            if (Hitting)
            {
                endEffect.transform.position = EndPoint;
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
                ? Vector3.Distance(transform.position, EndPoint)
                : maxLength;

            _pointsCount = Mathf.CeilToInt(length) + 1;
            lineRenderer.positionCount = _pointsCount;
        }
    }
}