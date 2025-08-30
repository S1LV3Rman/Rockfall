namespace S1LV3Rman.RockFall
{
    public interface IReusableInPool
    {
        public void PrepareForPulling();
        public void PrepareForReleasing();
    }
}