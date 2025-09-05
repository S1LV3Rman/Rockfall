namespace S1LV3Rman.RockFall.CoreGameplay
{
    public interface IPreDamageModifier : IDamageModifier
    {
        int PreModify(ref DamageContext ctx, int incoming);
    }
}