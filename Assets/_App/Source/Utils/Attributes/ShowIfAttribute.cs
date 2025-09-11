using UnityEngine;

namespace S1LV3Rman.RockFall
{
    public class ShowIfAttribute : PropertyAttribute
    {
        public string ConditionName { get; }
        public object CompareValue { get; }
        public bool Invert { get; }

        /// <summary>
        /// Show if boolean member is true.
        /// </summary>
        public ShowIfAttribute(string conditionName, bool invert = false)
        {
            ConditionName = conditionName;
            Invert = invert;
            CompareValue = null;
        }

        /// <summary>
        /// Show if member equals compareValue.
        /// </summary>
        public ShowIfAttribute(string conditionName, object compareValue, bool invert = false)
        {
            ConditionName = conditionName;
            CompareValue = compareValue;
            Invert = invert;
        }
    }
}