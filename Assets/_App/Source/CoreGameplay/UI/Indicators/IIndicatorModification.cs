namespace S1LV3Rman.RockFall.CoreGameplay
{
    public interface IIndicatorModification
    {
        public void AttachToIndicator(Indicator indicator);
        public void Remove();
    }
}