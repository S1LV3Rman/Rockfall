namespace S1LV3Rman.RockFall.CoreGameplay
{
    public interface IDamageModifier
    {
        void OnApplied(DamageContext ctx, int appliedAmount);
    }
}