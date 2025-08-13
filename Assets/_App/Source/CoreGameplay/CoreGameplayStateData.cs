namespace S1LV3Rman.RockFall
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