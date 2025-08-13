using System.Threading;
using Cysharp.Threading.Tasks;
using Eflatun.SceneReference;
using UnityEngine;

namespace S1LV3Rman.RockFall.App
{
    public abstract class SingleSceneAppState<TScope, TStateData> : IAppState<TStateData>
        where TScope : AppStateLifetimeScope<TStateData>
        where TStateData : IStateData
    {
        private readonly SceneChanger _sceneChanger;

        protected abstract SceneReference RequiredScene { get; }

        public SingleSceneAppState(
            SceneChanger sceneChanger
        )
        {
            _sceneChanger = sceneChanger;
        }

        public async UniTask EnterAsync(TStateData data, CancellationToken token)
        {
            await _sceneChanger.SwitchToScene(RequiredScene.Name, token);
            var lifetime = Object.FindFirstObjectByType<TScope>();
            if (lifetime == null)
                throw new StateEnteringException("There is no lifetime scope of type " + nameof(TScope));

            lifetime.SetData(data);
            lifetime.Build();
        }

        public async UniTask ExitAsync(CancellationToken token)
        {
            await _sceneChanger.UnloadSceneAsync(RequiredScene.Name, token);
        }
    }
}