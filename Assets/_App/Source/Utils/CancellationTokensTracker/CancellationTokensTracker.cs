using System;
using System.Collections.Generic;
using System.Threading;

namespace S1LV3Rman.RockFall
{
    public class CancellationTokensTracker : IDisposable
    {
        private readonly TimeSpan _defaultTimeout;
        
        private readonly HashSet<CancellationTokenSource> _trackedSources = new();
        private readonly object _lock = new();

        public CancellationTokensTracker(TimeSpan defaultTimeout)
        {
            _defaultTimeout = defaultTimeout;
        }

        public ScopedCancellationToken CreateToken()
        {
            var newCts = new CancellationTokenSource(_defaultTimeout);
            lock (_lock)
                _trackedSources.Add(newCts);

            return new ScopedCancellationToken(newCts, Untrack);
        }

        public ScopedCancellationToken Track(CancellationToken externalToken)
        {
            if (!externalToken.CanBeCanceled) 
                return CreateToken();
            
            var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(externalToken);
            lock (_trackedSources)
                _trackedSources.Add(linkedCts);

            return new ScopedCancellationToken(linkedCts, Untrack);
        }

        private void Untrack(CancellationTokenSource source)
        {
            lock (_lock)
                _trackedSources.Remove(source);
        }

        public void CancelAll()
        {
            lock (_lock)
            {
                foreach (var source in _trackedSources)
                    source.Cancel();
                foreach (var source in _trackedSources)
                    source.Dispose();
                _trackedSources.Clear();
            }
        }

        public void Dispose() => CancelAll();
    }
}