using System;
using UnityEditor;
using UnityEngine;

namespace S1LV3Rman.RockFall.Editor
{
    [CustomPropertyDrawer(typeof(OnFieldChangedAttribute))]
    public class OnFieldChangedAttributePropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.PropertyField(position, property, label);
            if (!EditorGUI.EndChangeCheck())
                return;

            property.serializedObject.ApplyModifiedProperties();

            var targetObject = property.serializedObject.targetObject;

            var fieldChangedAttribute = attribute as OnFieldChangedAttribute;
            var methodName = fieldChangedAttribute?.MethodName;
            if (methodName.IsNullOrWhiteSpace())
                return;

            var classType = targetObject.GetType();
            var methodInfo = classType.GetMethod(methodName);
            if (methodInfo is null)
                throw new InvalidOperationException($"OnFieldChanged: {classType.Name}.{methodName} is not exist!");

            var methodParameters = methodInfo.GetParameters();
            switch (methodParameters.Length)
            {
                case 0:
                    methodInfo.Invoke(targetObject, null);
                    break;
                case 1:
                    methodInfo.Invoke(targetObject, new[] {property.GetValue()});
                    break;
                default:
                    throw new InvalidOperationException(
                        $"OnFieldChanged: {classType.Name}.{methodName} cannot be called for {property.name}.\n" +
                        $"Use a method with no arguments or a single argument of type {property.type}");
            }
        }
    }
}