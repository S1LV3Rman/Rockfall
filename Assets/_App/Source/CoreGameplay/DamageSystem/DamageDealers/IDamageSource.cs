using System;

namespace S1LV3Rman.RockFall.CoreGameplay
{
    public interface IDamageSource
    {
        public Guid Id { get; }
        public int TeamId { get; }
    }
}