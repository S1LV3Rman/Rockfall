using UnityEngine;

namespace S1LV3Rman.RockFall
{
    public class ShowIfAttribute : PropertyAttribute
    {
        public string ConditionName { get; }
        public bool Invert { get; }

        public ShowIfAttribute(string conditionName, bool invert = false)
        {
            ConditionName = conditionName;
            Invert = invert;
        }
    }
}