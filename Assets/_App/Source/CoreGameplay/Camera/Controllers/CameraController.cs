using Unity.Cinemachine;
using UnityEngine;

namespace S1LV3Rman.RockFall.CoreGameplay
{
    public abstract class CameraController : MonoBehaviour
    {
        [SerializeField] private bool _isActive;
        [SerializeField] protected CinemachineCamera _virtualCamera;

        public bool IsActive => _isActive;

        public void Lock() => SetActive(false);
        public void Unlock() => SetActive(true);
        public virtual void SetActive(bool isActive) => _isActive = isActive;
    }
}