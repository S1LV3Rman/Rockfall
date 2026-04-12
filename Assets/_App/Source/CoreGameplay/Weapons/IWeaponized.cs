using System.Collections.Generic;

namespace S1LV3Rman.RockFall.CoreGameplay
{
    public interface IWeaponized
    {
        public AttackModifier Modifier { get; }
        public IReadOnlyCollection<IWeapon> Weapons {get;}
    }
}