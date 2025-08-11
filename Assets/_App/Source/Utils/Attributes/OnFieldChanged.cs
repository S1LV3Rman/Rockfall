using UnityEngine;

namespace S1LV3Rman.RockFall
{
    public class OnFieldChangedAttribute : PropertyAttribute
    {
        public readonly string MethodName;

        public OnFieldChangedAttribute(string methodNameNoArguments)
        {
            MethodName = methodNameNoArguments;
        }
    }
}