namespace S1LV3Rman.RockFall.CoreGameplay
{
    public interface IPostDamageModifier : IDamageModifier
    {
        int PostModify(ref DamageContext ctx, int incoming);
    }
}