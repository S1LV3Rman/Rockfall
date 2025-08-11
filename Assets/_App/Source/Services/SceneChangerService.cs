using System;
using Cysharp.Threading.Tasks;
using Eflatun.SceneReference;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace S1LV3Rman.RockFall
{
    public sealed class SceneChangerService
    {
        private readonly SceneReference _emptyScene;

        public SceneChangerService(AppScenesConfig appScenes)
        {
            _emptyScene = appScenes.EmptyScene;
        }

        public bool IsSceneActive(string sceneName) => SceneManager.GetActiveScene().name == sceneName;

        /// <summary>
        /// Выгружает текущую сцену и переключается на sceneName
        /// </summary>
        /// <param name="sceneName">название сцены, на которую надо переключиться</param>
        /// <param name="progress">(опционально) объект для отслеживания прогресса</param>
        public async UniTask SwitchToScene(string sceneName, IProgress<float> progress = null)
        {
            var oldScene = SceneManager.GetActiveScene();
            
            var addAndSwitchProgress = Progress.Create<float>(p => progress?.Report(0.5f * p));
            await AddSceneAndSwitchAsync(sceneName, addAndSwitchProgress);
            
            var unloadingProgress = Progress.Create<float>(p => progress?.Report(0.5f * p + 0.5f));
            await UnloadSceneAsync(oldScene.name, unloadingProgress);
        }

        /// <summary>
        /// Перезагружает сцену sceneName
        /// </summary>
        /// <param name="sceneName">название сцены, которую надо перезагрузить</param>
        /// <param name="progress">(опционально) объект для отслеживания прогресса</param>
        public async UniTask ReloadScene(string sceneName, IProgress<float> progress = null)
        {
            var unloadingProgress = Progress.Create<float>(p => progress?.Report(0.5f * p));
            var loadingProgress = Progress.Create<float>(p => progress?.Report(0.5f * p + 0.5f));
            if (SceneManager.GetActiveScene().name == sceneName)
            {
                await SwitchToScene(_emptyScene.Name, unloadingProgress);
                await SwitchToScene(sceneName, loadingProgress);
            }
            else
            {
                await UnloadSceneAsync(sceneName, unloadingProgress);
                await AddSceneAsync(sceneName, loadingProgress);
            }
        }

        /// <summary>
        /// Загружает сцену sceneName и переключается на неё, не удаляя текущую
        /// </summary>
        /// <param name="sceneName">название сцены, которую надо загрузить</param>
        /// <param name="progress">(опционально) объект для отслеживания прогресса</param>
        public async UniTask AddSceneAndSwitchAsync(string sceneName, IProgress<float> progress = null)
        {
            await AddSceneAsync(sceneName, progress);
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
        }

        /// <summary>
        /// Выгружает сцену sceneName
        /// </summary>
        /// <param name="sceneName">сцена, которую надо выгрузить</param>
        /// <param name="progress">(опционально) объект для отслеживания прогресса</param>
        public async UniTask UnloadSceneAsync(string sceneName, IProgress<float> progress = null)
        {
            if (SceneManager.GetActiveScene().name == sceneName)
            {
                await SwitchToScene(_emptyScene.Name, progress);
            }
            else
            {
                var unloading = SceneManager.UnloadSceneAsync(sceneName);
                await TrackProgress(unloading, progress);
            }
        }

        /// <summary>
        /// Загружает сцену sceneName, не переключаясь на неё
        /// </summary>
        /// <param name="sceneName">сцена, которую надо загрузить</param>
        /// <param name="progress">(опционально) объект для отслеживания прогресса</param>
        public async UniTask AddSceneAsync(string sceneName, IProgress<float> progress = null)
        {
            var requiredScene = SceneManager.GetSceneByName(sceneName);
            if (requiredScene.IsValid() && requiredScene.isLoaded)
            {
                var unloadingProgress = Progress.Create<float>(p => progress?.Report(0.5f * p));
                await UnloadSceneAsync(sceneName, unloadingProgress);
                
                var loadingProgress = Progress.Create<float>(p => progress?.Report(0.5f * p + 0.5f));
                var loading = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
                await TrackProgress(loading, loadingProgress);
            }
            else
            {
                var loading = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
                await TrackProgress(loading, progress);
            }
        }

        private async UniTask TrackProgress(AsyncOperation operation, IProgress<float> progress)
        {
            if (operation is null)
                return;
            
            while (!operation.isDone)
            {
                progress?.Report(operation.progress);
                await UniTask.NextFrame();
            }
            progress?.Report(1f);
        }
    }
}