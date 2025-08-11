using System;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace S1LV3Rman.RockFall.Editor
{
    public static class SerializedPropertyExtension
    {
        public enum GUIDirection
        {
            Horizontal,
            Vertical
        }

        private const BindingFlags ANY = BindingFlags.Instance
                                         | BindingFlags.Static
                                         | BindingFlags.Public
                                         | BindingFlags.NonPublic;

        public static object GetValue(this SerializedProperty property)
        {
            var targetObject = property.serializedObject.targetObject;
            var classType = targetObject.GetType();
            var field = GetField(classType, property.name);
            return field.GetValue(targetObject);
        }

        private static FieldInfo GetField(Type classType, string fieldName)
        {
            while (classType is not null)
            {
                var field = classType.GetField(fieldName, ANY);
                if (field is not null)
                    return field;

                classType = classType.BaseType;
            }

            return null;
        }

        public static bool IsInsideOfArray(this SerializedProperty property) =>
            Regex.IsMatch(property.propertyPath, @"\.Array\.data\[\d+\]$");

        public static Rect Draw(this SerializedProperty property,
            Rect position,
            string label = null,
            GUIDirection direction = GUIDirection.Vertical)
        {
            position.height = EditorGUI.GetPropertyHeight(property);
            EditorGUI.PropertyField(position, property, new GUIContent(label ?? property.displayName));
            if (direction == GUIDirection.Vertical)
                position.y += position.height;
            else
                position.x += position.width;
            return position;
        }

        public static Rect DrawWithoutLabel(this SerializedProperty property,
            Rect position,
            GUIDirection direction = GUIDirection.Vertical)
        {
            position.height = EditorGUI.GetPropertyHeight(property);

            // Save current indent and reset it to avoid extra offset.
            var indentLevel = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            EditorGUI.PropertyField(position, property, GUIContent.none);
            EditorGUI.indentLevel = indentLevel;

            if (direction == GUIDirection.Vertical)
                position.y += position.height;
            else
                position.x += position.width;
            return position;
        }
    }
}