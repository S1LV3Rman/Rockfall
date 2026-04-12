using UnityEngine;

namespace S1LV3Rman.RockFall.CoreGameplay
{
    public interface IWeapon : IDamageDealer
    {
        public float ProjectileSpeed { get; }
        public float MaxFireDistance { get; }
        public Vector3 Position { get; }
        public Vector3 Direction { get; set; }
    }
}