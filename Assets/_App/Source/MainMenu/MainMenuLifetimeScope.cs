using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace S1LV3Rman.RockFall
{
    public sealed class MainMenuLifetimeScope : AppStateLifetimeScope<MainMenuStateData>
    {
        [Header("UI")]
        [SerializeField] private MainMenuUI _mainMenuUI;
        
        // [Header("Configs")]

        protected override void ConfigureState(IContainerBuilder builder)
        {
            builder.RegisterInstance(_mainMenuUI);

            builder.RegisterEntryPoint<MainMenuRunner>();
        }
    }
}