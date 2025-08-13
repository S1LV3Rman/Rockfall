using System.Threading;
using Cysharp.Threading.Tasks;

namespace S1LV3Rman.RockFall.App
{
    public interface IAppState
    {
        UniTask ExitAsync(CancellationToken token);
    }

    public interface IAppState<in TData> : IAppState where TData : IStateData
    {
        UniTask EnterAsync(TData data, CancellationToken token);
    }
}