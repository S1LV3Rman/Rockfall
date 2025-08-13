using System;
using System.Threading;

namespace S1LV3Rman.RockFall
{
    public sealed class ScopedCancellationToken : IDisposable
    {
        public CancellationToken Token => _cts.Token;

        private readonly CancellationTokenSource _cts;
        private readonly Action<CancellationTokenSource> _onDispose;
        private bool _disposed;

        public ScopedCancellationToken(CancellationTokenSource cts, Action<CancellationTokenSource> onDispose)
        {
            _cts = cts;
            _onDispose = onDispose;
        }

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;
            _onDispose(_cts);
            _cts.Dispose();
        }
    }
}