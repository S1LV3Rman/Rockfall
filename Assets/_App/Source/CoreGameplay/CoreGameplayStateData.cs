using S1LV3Rman.RockFall.App;

namespace S1LV3Rman.RockFall.CoreGameplay
{
    public struct CoreGameplayStateData : IStateData
    {
        public readonly WeaponType WeaponType;

        public CoreGameplayStateData(WeaponType weaponType) : this()
        {
            WeaponType = weaponType;
        }
    }
}