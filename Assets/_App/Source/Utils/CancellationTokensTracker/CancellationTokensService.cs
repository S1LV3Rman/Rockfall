using System;
using System.Collections.Generic;

namespace S1LV3Rman.RockFall
{
    public sealed class CancellationTokensService : IDisposable
    {
        private readonly TimeSpan _defaultTimeout;

        private readonly CancellationTokensTracker _globalTracker;
        private readonly List<CancellationTokensTracker> _trackers = new();

        public CancellationTokensService(ICancellationTokensTrackerSettings settings)
        {
            _defaultTimeout = settings.DefaultTimeout;
            _globalTracker = new CancellationTokensTracker(_defaultTimeout);
        }

        public CancellationTokensTracker CreateTracker()
        {
            var tracker = new CancellationTokensTracker(_defaultTimeout);
            _trackers.Add(tracker);
            return tracker;
        }

        public ScopedCancellationToken CreateToken() => _globalTracker.CreateToken();

        public void CancelAll()
        {
            foreach (var tracker in _trackers) 
                tracker.CancelAll();
            
            _globalTracker.CancelAll();
        }
        
        public void Dispose()
        {
            foreach (var tracker in _trackers) 
                tracker.Dispose();
            
            _trackers.Clear();
            
            _globalTracker.Dispose();
        }
    }
}