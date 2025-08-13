using UnityEngine;
using VContainer.Unity;

namespace S1LV3Rman.RockFall
{
    public sealed class CoreGameplayRunner : IInitializable
    {
        private readonly CoreGameplayStateData _stateData;

        public CoreGameplayRunner(
            CoreGameplayStateData stateData
            )
        {
            _stateData = stateData;
        }

        public void Initialize()
        {
            Debug.Log(_stateData.WeaponType + " was selected");
        }
    }
}