using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Eflatun.SceneReference;
using S1LV3Rman.RockFall.App;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace S1LV3Rman.RockFall
{
    public sealed class SceneChanger
    {
        private readonly SceneReference _emptyScene;

        public SceneChanger(AppScenesConfig appScenes)
        {
            _emptyScene = appScenes.EmptyScene;
        }

        public bool IsSceneActive(string sceneName) => SceneManager.GetActiveScene().name == sceneName;

        /// <summary>
        /// Выгружает текущую сцену и переключается на sceneName
        /// </summary>
        /// <param name="sceneName">название сцены, на которую надо переключиться</param>
        /// <param name="progress">(опционально) объект для отслеживания прогресса</param>
        public async UniTask SwitchToScene(
            string sceneName,
            CancellationToken cancellationToken = default,
            IProgress<float> progress = null)
        {
            var oldScene = SceneManager.GetActiveScene();
            
            var addAndSwitchProgress = Progress.Create<float>(p => progress?.Report(0.5f * p));
            await AddSceneAndSwitchAsync(sceneName, cancellationToken, addAndSwitchProgress);
            
            var unloadingProgress = Progress.Create<float>(p => progress?.Report(0.5f * p + 0.5f));
            await UnloadSceneAsync(oldScene.name, cancellationToken, unloadingProgress);
        }

        /// <summary>
        /// Перезагружает сцену sceneName
        /// </summary>
        /// <param name="sceneName">название сцены, которую надо перезагрузить</param>
        /// <param name="progress">(опционально) объект для отслеживания прогресса</param>
        public async UniTask ReloadScene(
            string sceneName,
            CancellationToken cancellationToken = default,
            IProgress<float> progress = null)
        {
            var unloadingProgress = Progress.Create<float>(p => progress?.Report(0.5f * p));
            var loadingProgress = Progress.Create<float>(p => progress?.Report(0.5f * p + 0.5f));
            if (SceneManager.GetActiveScene().name == sceneName)
            {
                await SwitchToScene(_emptyScene.Name, cancellationToken, unloadingProgress);
                await SwitchToScene(sceneName, cancellationToken, loadingProgress);
            }
            else
            {
                await UnloadSceneAsync(sceneName, cancellationToken, unloadingProgress);
                await AddSceneAsync(sceneName, cancellationToken, loadingProgress);
            }
        }

        /// <summary>
        /// Загружает сцену sceneName и переключается на неё, не удаляя текущую
        /// </summary>
        /// <param name="sceneName">название сцены, которую надо загрузить</param>
        /// <param name="progress">(опционально) объект для отслеживания прогресса</param>
        public async UniTask AddSceneAndSwitchAsync(
            string sceneName,
            CancellationToken cancellationToken = default,
            IProgress<float> progress = null)
        {
            await AddSceneAsync(sceneName, cancellationToken, progress);
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
        }

        /// <summary>
        /// Выгружает сцену sceneName
        /// </summary>
        /// <param name="sceneName">сцена, которую надо выгрузить</param>
        /// <param name="progress">(опционально) объект для отслеживания прогресса</param>
        public async UniTask UnloadSceneAsync(
            string sceneName,
            CancellationToken cancellationToken = default,
            IProgress<float> progress = null)
        {
            if (SceneManager.GetActiveScene().name == sceneName)
            {
                await SwitchToScene(_emptyScene.Name, cancellationToken, progress);
            }
            else
            {
                var unloading = SceneManager.UnloadSceneAsync(sceneName);
                await TrackProgress(unloading, cancellationToken, progress);
            }
        }

        /// <summary>
        /// Загружает сцену sceneName, не переключаясь на неё
        /// </summary>
        /// <param name="sceneName">сцена, которую надо загрузить</param>
        /// <param name="progress">(опционально) объект для отслеживания прогресса</param>
        public async UniTask AddSceneAsync(
            string sceneName,
            CancellationToken cancellationToken = default,
            IProgress<float> progress = null)
        {
            var requiredScene = SceneManager.GetSceneByName(sceneName);
            if (requiredScene.IsValid() && requiredScene.isLoaded)
            {
                var unloadingProgress = Progress.Create<float>(p => progress?.Report(0.5f * p));
                await UnloadSceneAsync(sceneName, cancellationToken, unloadingProgress);
                
                var loadingProgress = Progress.Create<float>(p => progress?.Report(0.5f * p + 0.5f));
                var loading = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
                await TrackProgress(loading, cancellationToken, loadingProgress);
            }
            else
            {
                var loading = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
                await TrackProgress(loading, cancellationToken, progress);
            }
        }

        private async UniTask TrackProgress(
            AsyncOperation operation,
            CancellationToken cancellationToken,
            IProgress<float> progress)
        {
            if (operation is null)
                return;
            
            while (!operation.isDone)
            {
                progress?.Report(operation.progress);
                await UniTask.NextFrame(cancellationToken);
            }

            progress?.Report(1f);
        }
    }
}