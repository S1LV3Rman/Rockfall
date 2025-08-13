using System;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;
using VContainer.Unity;

namespace S1LV3Rman.RockFall
{
    public class MainMenuRunner : IInitializable, IDisposable
    {
        private readonly MainMenuUI _mainMenuUI;
        private readonly AppStateChanger _stateChanger;

        private IDisposable _weaponSelection;
        
        public MainMenuRunner(
            MainMenuUI mainMenuUI,
            AppStateChanger stateChanger
            )
        {
            _mainMenuUI = mainMenuUI;
            _stateChanger = stateChanger;
        }

        public void Initialize()
        {
            _weaponSelection = _mainMenuUI.WeaponSelection.Subscribe(EnterCoreGameplay);
        }

        private void EnterCoreGameplay(WeaponType weaponType)
        {
            var stateData = new CoreGameplayStateData(weaponType);
            _stateChanger.ChangeStateAsync<CoreGameplayState, CoreGameplayStateData>(stateData).Forget();
        }

        public void Dispose()
        {
            _weaponSelection.Dispose();
        }
    }
}