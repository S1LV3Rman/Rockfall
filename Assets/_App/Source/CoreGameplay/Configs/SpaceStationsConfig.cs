using UnityEngine;

namespace S1LV3Rman.RockFall.CoreGameplay
{
    [CreateAssetMenu(fileName = nameof(SpaceStationsConfig), menuName = "Config/" + nameof(SpaceStationsConfig), order = 0)]
    public class SpaceStationsConfig : ScriptableObject
    {
        public SpaceStation BasicStation;
    }
}