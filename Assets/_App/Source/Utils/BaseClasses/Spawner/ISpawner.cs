using R3;

namespace S1LV3Rman.RockFall
{
    public interface ISpawner<TRequest> where TRequest : ISpawnRequest
    {
        Observable<TRequest> Requests { get; }
        void SetActive(bool active);
    }
}