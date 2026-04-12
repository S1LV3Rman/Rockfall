using System;
using Cysharp.Threading.Tasks;
using R3;
using S1LV3Rman.RockFall.App;
using S1LV3Rman.RockFall.CoreGameplay;
using VContainer.Unity;

namespace S1LV3Rman.RockFall.MainMenu
{
    public class MainMenuLogic : IInitializable, IDisposable
    {
        private readonly TitleUI _titleUI;
        private readonly WeaponSelectionUI _weaponSelectionUI;
        private readonly MainMenuStateData _stateData;
        private readonly AppStateChanger _stateChanger;

        private IDisposable _subscriptions;
        
        public MainMenuLogic(
            TitleUI titleUI,
            WeaponSelectionUI weaponSelectionUI,
            MainMenuStateData stateData,
            AppStateChanger stateChanger
            )
        {
            _titleUI = titleUI;
            _weaponSelectionUI = weaponSelectionUI;
            _stateData = stateData;
            _stateChanger = stateChanger;
        }

        public void Initialize()
        {
            var start = _titleUI.StartPress.Subscribe(ChooseWeapon);
            var weaponSelection = _weaponSelectionUI.WeaponSelection.Subscribe(EnterCoreGameplay);
            _subscriptions = Disposable.Combine(start, weaponSelection);

            if (_stateData.IsRetrying)
                _weaponSelectionUI.Open();
            else
                _titleUI.Open();
        }

        private void ChooseWeapon(Unit _)
        {
            _titleUI.Close();
            _weaponSelectionUI.Open();
        }

        private void EnterCoreGameplay(string weaponType)
        {
            var stateData = new CoreGameplayStateData(weaponType);
            _stateChanger.ChangeStateAsync<CoreGameplayState, CoreGameplayStateData>(stateData).Forget();
        }

        public void Dispose()
        {
            _subscriptions.Dispose();
        }
    }
}