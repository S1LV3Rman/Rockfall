using UnityEditor;
using UnityEngine;

namespace S1LV3Rman.RockFall.Editor
{
    public abstract class PropertyDrawerLabeledOnTop : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            EditorGUI.BeginChangeCheck();

            var nested = property.IsInsideOfArray() || label == GUIContent.none;

            if (!nested)
            {
                EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
                position.y += GUISizes.LineWithSpacing;
                position.height -= GUISizes.LineWithSpacing;
                EditorGUI.indentLevel++;
            }

            Draw(position, property);

            if (!nested) EditorGUI.indentLevel--;

            if (EditorGUI.EndChangeCheck())
                property.serializedObject.ApplyModifiedProperties();
            EditorGUI.EndProperty();
        }

        protected abstract void Draw(Rect position, SerializedProperty property);

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var height = GetPropertyHeightWithoutLabel(property);
            if (property.IsInsideOfArray() || label == GUIContent.none)
                height += GUISizes.LineWithSpacing;
            return height;
        }

        protected abstract float GetPropertyHeightWithoutLabel(SerializedProperty property);
    }
}