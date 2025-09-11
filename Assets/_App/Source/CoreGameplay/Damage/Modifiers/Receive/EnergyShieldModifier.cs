using System;
using UnityEngine;

namespace S1LV3Rman.RockFall.CoreGameplay
{
    public sealed class EnergyShieldModifier : IDamageModifier
    {
        private readonly Func<int> _getCurrentShield;
        private readonly Action<int> _setCurrentShield;

        public EnergyShieldModifier(
            Func<int> getCurrentShield,
            Action<int> setCurrentShield)
        {
            _getCurrentShield = getCurrentShield;
            _setCurrentShield = setCurrentShield;
        }

        public int Modify(ref DamageContext context, int incoming)
        {
            var currentShield = _getCurrentShield.Invoke();
            var absorbed = Mathf.Min(currentShield, incoming);
            _setCurrentShield.Invoke(currentShield - absorbed);
            return incoming - absorbed;
        }

        public void OnApplied(DamageContext context, int applied)
        {
        }
    }
}