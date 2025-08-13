using S1LV3Rman.RockFall.App;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace S1LV3Rman.RockFall.MainMenu
{
    public sealed class MainMenuLifetimeScope : AppStateLifetimeScope<MainMenuStateData>
    {
        [Header("UI")]
        [SerializeField] private TitleUI _titleUI;
        [SerializeField] private WeaponSelectionUI _weaponSelectionUI;
        
        // [Header("Configs")]

        protected override void ConfigureState(IContainerBuilder builder)
        {
            builder.RegisterInstance(_titleUI);
            builder.RegisterInstance(_weaponSelectionUI);

            builder.RegisterEntryPoint<MainMenuLogic>();
        }
    }
}