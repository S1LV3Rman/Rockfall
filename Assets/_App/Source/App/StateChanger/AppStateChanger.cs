using System.Threading;
using Cysharp.Threading.Tasks;
using VContainer;

namespace S1LV3Rman.RockFall.App
{
    public sealed class AppStateChanger
    {
        private readonly IObjectResolver _resolver;
        private IAppState _currentState;
        private CancellationTokenSource _cts;

        public AppStateChanger(IObjectResolver resolver)
        {
            _resolver = resolver;
        }

        public async UniTask ChangeStateAsync<TState, TData>(TData data)
            where TState : IAppState<TData>
            where TData : IStateData
        {
            _cts?.Cancel();
            _cts = new CancellationTokenSource();

            if (_currentState != null)
                await _currentState.ExitAsync(_cts.Token);

            var newState = _resolver.Resolve<TState>();
            _currentState = newState;
            await newState.EnterAsync(data, _cts.Token);
        }
    }
}