using UnityEngine;

namespace S1LV3Rman.RockFall.CoreGameplay
{
    public sealed class AimPoint : AliveTrackedMonoBehaviour
    {
        [field: SerializeField] public IndicatorTarget Indicator { get; private set; }
    }
}