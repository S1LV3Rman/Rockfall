using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace S1LV3Rman.RockFall
{
    public sealed class MainMenuLifetimeScope : LifetimeScope
    {
        [Header("UI")]
        [SerializeField] private MainMenuUI _mainMenuUI;
        
        // [Header("Configs")]

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(_mainMenuUI);
            
            
        }
    }
}