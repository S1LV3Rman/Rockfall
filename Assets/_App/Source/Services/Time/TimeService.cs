using UnityEngine;

namespace S1LV3Rman.RockFall
{
    public class TimeService
    {
        public float TimeScale
        {
            get => Time.timeScale;
            set => Time.timeScale = value;
        }
    }
}