using UnityEngine;

namespace S1LV3Rman.RockFall.CoreGameplay
{
    public sealed class HitBox : MonoBehaviour
    {
        [field: SerializeField] public IDamageable Owner { get; private set; }

        private void Awake()
        {
            if (Owner == null)
                Owner = GetComponentInParent<IDamageable>();
        }
    }
}