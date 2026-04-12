namespace S1LV3Rman.RockFall.CoreGameplay
{
    public interface IAttackModifier
    {
        public int Modify(ref AttackContext context);
    }
}