namespace S1LV3Rman.RockFall.CoreGameplay
{
    public readonly struct DamageEvent
    {
        public readonly DamageContext Context;
        public readonly int AppliedAmount; // after mitigation
        public readonly int HealthAfter;

        public DamageEvent(DamageContext ctx, int applied, int hpAfter)
        {
            Context = ctx;
            AppliedAmount = applied;
            HealthAfter = hpAfter;
        }
    }
}