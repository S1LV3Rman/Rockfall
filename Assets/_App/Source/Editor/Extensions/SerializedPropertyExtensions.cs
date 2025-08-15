using System;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR
#endif

namespace S1LV3Rman.RockFall
{
    public enum GUIFlow
    {
        Horizontal,
        Vertical
    }

    public static class SerializedPropertyExtensions
    {
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


        public static void Draw(this SerializedProperty property,
            ref Rect position,
            string label = null,
            string tooltip = null,
            GUIFlow flow = GUIFlow.Vertical,
            bool indent = false)
        {
            property.Draw(ref position, new GUIContent(label ?? property.displayName, tooltip), flow, indent);
        }

        public static void Draw(this SerializedProperty property,
            ref Rect position,
            GUIContent label = null,
            GUIFlow flow = GUIFlow.Vertical,
            bool indent = false)
        {
            var content = label ?? GUIContent.none;
            position.height = EditorGUI.GetPropertyHeight(property, content);

            if (indent) EditorGUI.indentLevel++;
            EditorGUI.PropertyField(position, property, content);
            if (indent) EditorGUI.indentLevel--;

            if (flow == GUIFlow.Vertical)
                position.y += position.height;
            else
                position.x += position.width;
        }

        public static bool DrawWithFoldout(this SerializedProperty property,
            ref Rect position,
            string label = null,
            string tooltip = null,
            GUIFlow flow = GUIFlow.Vertical)
        {
            var isComplex = property.hasVisibleChildren;
            var isExpanded = property.isExpanded;
            var labelContent = new GUIContent(label ?? property.displayName, tooltip);

            // Adjust height based on whether child fields are shown
            position.height = isComplex && isExpanded
                ? EditorGUIUtility.singleLineHeight + EditorGUI.GetPropertyHeight(property, GUIContent.none)
                : EditorGUIUtility.singleLineHeight;

            if (isComplex && !isExpanded)
            {
                property.isExpanded = EditorGUI.Foldout(position,
                    false, labelContent, true, EditorStyles.foldoutHeader);
            }
            else
            {
                var foldoutWidth = EditorGUIUtility.labelWidth + 2f;

                var foldoutPosition = position;
                var propertyPosition = position;

                if (isComplex)
                {
                    foldoutPosition.height = EditorGUIUtility.singleLineHeight;
                    propertyPosition.y += EditorGUIUtility.singleLineHeight;
                    propertyPosition.height -= EditorGUIUtility.singleLineHeight;
                }
                else
                {
                    foldoutPosition.width = foldoutWidth;
                    propertyPosition.x += foldoutWidth;
                    propertyPosition.width -= foldoutWidth;
                }

                property.isExpanded = EditorGUI.Foldout(foldoutPosition,
                    isExpanded, labelContent, true, EditorStyles.foldoutHeader);

                var indent = EditorGUI.indentLevel;
                EditorGUI.indentLevel = isComplex ? indent + 1 : 0;
                EditorGUI.PropertyField(propertyPosition, property, GUIContent.none);
                EditorGUI.indentLevel = indent;
            }

            if (flow == GUIFlow.Vertical)
                position.y += position.height;
            else
                position.x += position.width;
            return property.isExpanded;
        }

        public static void Draw(this SerializedProperty property,
            string label = null,
            string tooltip = null,
            bool indent = false)
        {
            if (indent) EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(property, new GUIContent(label ?? property.displayName, tooltip));
            if (indent) EditorGUI.indentLevel--;
        }

        public static void DrawWithoutLabel(this SerializedProperty property,
            ref Rect position,
            GUIFlow flow = GUIFlow.Vertical,
            bool indent = false)
        {
            position.height = EditorGUI.GetPropertyHeight(property, GUIContent.none);

            if (indent) EditorGUI.indentLevel++;
            EditorGUI.PropertyField(position, property, GUIContent.none);
            if (indent) EditorGUI.indentLevel--;

            if (flow == GUIFlow.Vertical)
                position.y += position.height;
            else
                position.x += position.width;
        }

        public static void DrawWithoutLabel(this SerializedProperty property, bool indent = false)
        {
            if (indent) EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(property, GUIContent.none);
            if (indent) EditorGUI.indentLevel--;
        }
    }
}