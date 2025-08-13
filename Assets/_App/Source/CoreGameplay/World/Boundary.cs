using UnityEngine;

namespace S1LV3Rman.RockFall.CoreGameplay
{
    public sealed class Boundary : MonoBehaviour
    {
        [field: SerializeField] public float WarningRadius { get; } = 400.0f;
        [field: SerializeField] public float DestroyRadius { get; } = 450.0f;

        public void OnDrawGizmosSelected()
        {
            // Желтым цветом рисуем сферу предупреждения
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, WarningRadius);

            // ...а красным — сферу уничтожения
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, DestroyRadius);
        }
    }
}