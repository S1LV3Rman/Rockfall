using UnityEngine;

namespace S1LV3Rman.RockFall.CoreGameplay
{
    public sealed class GameplayCamera : MonoBehaviour
    {
        public Camera Camera;
        public CameraFollowTarget _follow;
        public CameraLensShift _lensShift;

        public void SetControlsActive(bool isActive)
        {
            if (_follow != null) _follow.SetActive(isActive);
            if (_lensShift != null) _lensShift.SetActive(isActive);
        }

        public Vector2 WorldToScreenPoint(Vector3 worldPoint) =>
            Camera.WorldToScreenPoint(worldPoint);

        public Vector3 WorldToViewportPoint(Vector3 worldPoint)
        {
            var result = Camera.WorldToViewportPoint(worldPoint);
            result.x = Mathf.Clamp(result.x, 0, 1);
            result.y = Mathf.Clamp(result.y, 0, 1);
            return result;
        }

        public Vector3 ScreenToViewportPoint(Vector2 screenPoint)
        {
            var result = Camera.ScreenToViewportPoint(screenPoint);
            result.x = Mathf.Clamp(result.x, 0, 1);
            result.y = Mathf.Clamp(result.y, 0, 1);
            return result;
        }

        public Vector3 ScreenToWorldPoint(Vector2 screenPoint)
        {
            var point = new Vector3(screenPoint.x, screenPoint.y, Camera.nearClipPlane);
            var result = Camera.ScreenToWorldPoint(point);
            return result;
        }

        public Vector3 ScreenToWorldPointOnGround(Vector2 screenPoint)
        {
            var ray = Camera.ScreenPointToRay(screenPoint);
            var slope = -ray.direction.y;

            if (slope != 0.0f)
            {
                var scale = ray.origin.y / slope;

                return ray.GetPoint(scale);
            }

            return new Vector3(ray.origin.x, ray.origin.y, 0);
        }

        public Vector3 FromCameraDirection(Vector3 direction) =>
            Camera.transform.TransformVector(direction);

        public Vector3 FromCameraFlattenDirection(Vector3 direction) =>
            Vector3.ProjectOnPlane(FromCameraDirection(direction), Vector3.up).normalized;
    }
}