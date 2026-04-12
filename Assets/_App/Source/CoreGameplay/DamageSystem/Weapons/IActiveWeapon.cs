namespace S1LV3Rman.RockFall.CoreGameplay
{
    public interface IActiveWeapon : IWeapon
    {
        public void StartFire();
        public void StopFire();
    }
}