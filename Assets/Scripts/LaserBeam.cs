using System;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(LineRenderer))]
public class LaserBeam : MonoBehaviour
{

    public float noise = 1.0f;
    public float maxLength = 50.0f;

    //The particle system, in this case sparks which will be created by the Laser
    public ParticleSystem endEffect;

    public LayerMask layerMask = 0;

    private LineRenderer lineRenderer;
    private int currentLength;
    private Vector3 endPoint;
    public bool hitting { get; private set; } = false;
    public GameObject hitedObject { get; private set; }

    // Use this for initialization
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        endPoint = transform.position + transform.forward * maxLength;
    }

    void FixedUpdate()
    {
        UpdateRay();
    }

    void UpdateRay()
    {
        // Raycast from the location of the cube forwards
        if (Physics.Raycast(transform.position, transform.forward,
            out var hit, maxLength, layerMask, QueryTriggerInteraction.Ignore))
        {
            endPoint = hit.point;
            hitting = true;
            hitedObject = hit.collider.gameObject;
        }
        else
        {
            endPoint = transform.position + transform.forward * maxLength;
            hitting = false;
        }
    }

    void Update()
    {
        RenderLaser();
    }

    void RenderLaser()
    {
        // Shoot our laser forwards!
        UpdateLength();

        lineRenderer.SetPosition(0, transform.position);
        // Move through the Array
        for (int i = 1; i < currentLength; i++)
        {
            // Set the position here to the current location and
            // project it in the forward direction of the object it is attached to
            var circle = Random.insideUnitCircle * noise;
            var point = transform.right * circle.x + transform.up * circle.y;
            var offset = transform.position + i * transform.forward + point;

            lineRenderer.SetPosition(i, offset);
        }
    }
    
    void UpdateLength()
    {
        // Raycast from the location of the cube forwards
        if (hitting)
        {
            currentLength = (int) Mathf.Round((endPoint - transform.position).magnitude);
            lineRenderer.positionCount = currentLength + 1;
            lineRenderer.SetPosition(currentLength, endPoint);

            // Move our End Effect particle system to the hit point and start playing it
            if (endEffect)
            {
                endEffect.transform.position = endPoint;
                if (!endEffect.isPlaying)
                    endEffect.Play();
            }
        }
        else
        {
            currentLength = (int) Mathf.Round(maxLength) + 1;
            lineRenderer.positionCount = currentLength;

            // If we're not hitting anything, don't play the particle effects
            if (endEffect)
            {
                if (endEffect.isPlaying)
                    endEffect.Stop();
            }
        }
    }
}