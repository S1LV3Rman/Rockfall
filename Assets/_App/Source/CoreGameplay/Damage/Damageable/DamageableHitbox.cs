using UnityEngine;

namespace S1LV3Rman.RockFall.CoreGameplay
{
    public sealed class DamageableHitbox : MonoBehaviour
    {
        [SerializeField] private GameObject _owner;
        public IDamageable Owner { get; private set; }

        private void Awake() => Owner = _owner != null
            ? _owner.GetComponent<IDamageable>()
            : GetComponentInParent<IDamageable>();
    }
}