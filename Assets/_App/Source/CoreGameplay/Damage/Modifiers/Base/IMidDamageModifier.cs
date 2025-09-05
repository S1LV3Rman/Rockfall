namespace S1LV3Rman.RockFall.CoreGameplay
{
    public interface IMidDamageModifier : IDamageModifier
    {
        int MidModify(ref DamageContext ctx, int incoming);
    }
}