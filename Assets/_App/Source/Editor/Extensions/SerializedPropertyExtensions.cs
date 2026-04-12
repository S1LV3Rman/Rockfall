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
        None,
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

        private static Rect MoveWithFlow(Rect position, GUIFlow flow)
        {
            switch (flow)
            {
                case GUIFlow.None:
                    break;
                case GUIFlow.Horizontal:
                    position.x += position.width + GUISizes.HSpacing;
                    break;
                case GUIFlow.Vertical:
                    position.y += position.height + GUISizes.VSpacing;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(flow), flow, null);
            }

            return position;
        }

        #region Positioned

        public static void DrawWithoutLabel(this SerializedProperty property,
            ref Rect position,
            GUIFlow flow = GUIFlow.Vertical,
            bool indent = false,
            bool isReadonly = false)
        {
            property.Draw(ref position, GUIContent.none, flow, indent, isReadonly);
        }

        public static void Draw(this SerializedProperty property,
            ref Rect position,
            string label = null,
            string tooltip = null,
            GUIFlow flow = GUIFlow.Vertical,
            bool indent = false,
            bool isReadonly = false)
        {
            property.Draw(ref position,
                new GUIContent(label ?? property.displayName, tooltip), flow, indent, isReadonly);
        }

        public static void Draw(this SerializedProperty property,
            ref Rect position,
            GUIContent content,
            GUIFlow flow = GUIFlow.Vertical,
            bool indent = false,
            bool isReadonly = false)
        {
            content ??= GUIContent.none;
            position.height = EditorGUI.GetPropertyHeight(property, content);

            if (indent) EditorGUI.indentLevel++;
            var guiEnabled = GUI.enabled;
            if (isReadonly) GUI.enabled = false;

            EditorGUI.PropertyField(position, property, content);

            if (isReadonly) GUI.enabled = guiEnabled;
            if (indent) EditorGUI.indentLevel--;

            position = MoveWithFlow(position, flow);
        }

        public static bool DrawWithFoldout(this SerializedProperty property,
            ref Rect position,
            string label = null,
            string tooltip = null,
            GUIFlow flow = GUIFlow.Vertical,
            bool isReadonly = false)
        {
            return property.DrawWithFoldout(ref position,
                new GUIContent(label ?? property.displayName, tooltip), flow, isReadonly);
        }

        public static bool DrawWithFoldout(this SerializedProperty property,
            ref Rect position,
            GUIContent content,
            GUIFlow flow = GUIFlow.Vertical,
            bool isReadonly = false)
        {
            var isComplex = property.hasVisibleChildren;
            var isExpanded = property.isExpanded;

            // Adjust height based on whether child fields are shown
            position.height = isComplex && isExpanded
                ? GUISizes.LineWithSpacing + EditorGUI.GetPropertyHeight(property, GUIContent.none)
                : GUISizes.SingleLine;

            if (isComplex && !isExpanded)
            {
                property.isExpanded = EditorGUI.Foldout(position,
                    false, content, content != GUIContent.none, EditorStyles.foldoutHeader);
            }
            else
            {
                var foldoutPosition = position;
                var propertyPosition = position;

                if (isComplex)
                {
                    foldoutPosition.height = GUISizes.SingleLine;
                    propertyPosition.y += GUISizes.LineWithSpacing;
                    propertyPosition.height -= GUISizes.LineWithSpacing;
                }
                else
                {
                    foldoutPosition.width = GUISizes.LabelWidth;
                    propertyPosition.x += GUISizes.LabelWidth + GUISizes.HSpacing;
                    propertyPosition.width -= GUISizes.LabelWidth + GUISizes.HSpacing;
                }

                property.isExpanded = EditorGUI.Foldout(foldoutPosition,
                    isExpanded, content, content != GUIContent.none, EditorStyles.foldoutHeader);

                property.DrawWithoutLabel(ref propertyPosition, flow, isComplex, isReadonly);
            }

            position = MoveWithFlow(position, flow);
            return property.isExpanded;
        }

        #endregion

        #region Layouted

        public static void DrawWithoutLabel(this SerializedProperty property,
            bool indent = false,
            bool isReadonly = false)
        {
            property.Draw(GUIContent.none, indent, isReadonly);
        }

        public static void Draw(this SerializedProperty property,
            string label = null,
            string tooltip = null,
            bool indent = false,
            bool isReadonly = false)
        {
            property.Draw(new GUIContent(label ?? property.displayName, tooltip), indent, isReadonly);
        }

        public static void Draw(this SerializedProperty property,
            GUIContent content,
            bool indent = false,
            bool isReadonly = false)
        {
            content ??= GUIContent.none;

            if (indent) EditorGUI.indentLevel++;
            var guiEnabled = GUI.enabled;
            if (isReadonly) GUI.enabled = false;

            EditorGUILayout.PropertyField(property, content, true);

            if (isReadonly) GUI.enabled = guiEnabled;
            if (indent) EditorGUI.indentLevel--;
        }

        #endregion
    }
}