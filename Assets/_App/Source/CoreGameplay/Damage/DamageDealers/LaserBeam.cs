using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace S1LV3Rman.RockFall.CoreGameplay
{
    [RequireComponent(typeof(LineRenderer))]
    public class LaserBeam : MonoBehaviour, IDamageDealer
    {
        [SerializeField] private float noise = 1.0f;
        [SerializeField] private ParticleSystem endEffect;
        [SerializeField] private LineRenderer lineRenderer;
        [SerializeField] private LayerMask layerMask = 0;

        public IInstigator Source { get; private set; }
        public DamageType DamageType => DamageType.Laser;
        private Func<int> _getDamage;
        public int BaseDamage => _getDamage.Invoke();
        public DamageModifier Modifier { get; }

        public float MaxLength { get; private set; }
        public bool Hitting { get; private set; }
        public GameObject HittedObject { get; private set; }
        public Vector3 EndPoint { get; private set; }

        private int _pointsCount;

        public void Setup(IInstigator source, Func<int> getDamage, float fireDistance)
        {
            Source = source;
            _getDamage = getDamage;
            MaxLength = fireDistance;

            EndPoint = transform.TransformPoint(0f, 0f, MaxLength);
        }

        public bool TryDealDamage()
        {
            if (HittedObject == null)
                return false;

            var hitbox = HittedObject.GetComponent<DamageableHitbox>();
            if (hitbox == null)
                return false;

            var damageContext = new DamageContext(Source, this, hitbox.Owner, EndPoint,
                BaseDamage, DamageType, teamId: Source.TeamId);
            hitbox.Owner.ReceiveDamage(damageContext);

            return true;
        }

        private void FixedUpdate()
        {
            UpdateRay();
        }

        private void UpdateRay()
        {
            // Raycast from the location of the cube forwards
            Hitting = Physics.Raycast(transform.position, transform.forward,
                out var hit, MaxLength, layerMask, QueryTriggerInteraction.Ignore);

            if (Hitting)
            {
                EndPoint = hit.point;
                HittedObject = hit.collider.gameObject;
            }
            else
            {
                EndPoint = transform.TransformPoint(0f, 0f, MaxLength);
            }
        }

        private void Update()
        {
            RenderLaser();
            if (endEffect != null)
                UpdateEndEffect();
        }

        private void RenderLaser()
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

        private void UpdateLength()
        {
            var length = Hitting
                ? Vector3.Distance(transform.position, EndPoint)
                : MaxLength;

            _pointsCount = Mathf.CeilToInt(length) + 1;
            lineRenderer.positionCount = _pointsCount;
        }
    }
}