using System;
using UnityEngine;

namespace S1LV3Rman.RockFall.CoreGameplay
{
    public sealed class DamageResistModifier : IDamageModifier
    {
        private readonly DamageType _damageType;
        private readonly Func<float> _getResist;

        public DamageResistModifier(
            DamageType damageType,
            Func<float> getResist)
        {
            _damageType = damageType;
            _getResist = getResist;
        }

        public int Modify(ref DamageContext context, int incoming)
        {
            if (context.DamageType != _damageType)
                return incoming;

            var resist = _getResist.Invoke();
            var reduced = Mathf.RoundToInt(incoming * resist);
            return incoming - reduced;
        }

        public void OnApplied(DamageContext context, int applied)
        {
        }
    }
}