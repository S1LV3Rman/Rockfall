using S1LV3Rman.RockFall.App;

namespace S1LV3Rman.RockFall.CoreGameplay
{
    public struct CoreGameplayStateData : IStateData
    {
        public readonly string WeaponType;

        public CoreGameplayStateData(string weaponType) : this()
        {
            WeaponType = weaponType;
        }
    }
}