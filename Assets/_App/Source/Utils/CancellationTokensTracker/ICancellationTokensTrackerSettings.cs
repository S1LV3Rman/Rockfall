using System;

namespace S1LV3Rman.RockFall
{
    public interface ICancellationTokensTrackerSettings
    {
        public TimeSpan DefaultTimeout { get; }
    }
}