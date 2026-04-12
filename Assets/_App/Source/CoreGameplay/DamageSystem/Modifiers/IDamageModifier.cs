namespace S1LV3Rman.RockFall.CoreGameplay
{
    public interface IDamageModifier
    {
        public int Modify(ref DamageContext context, int incoming);
        public void OnApplied(DamageContext context, int applied);
    }
}