using UnityEditor;
using UnityEngine;

namespace S1LV3Rman.RockFall.Editor
{
    public abstract class PropertyDrawerLabeledOnLeft : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            EditorGUI.BeginChangeCheck();

            var nested = property.IsInsideOfArray() || label == GUIContent.none;

            if (!nested)
                position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            Draw(position, property);

            if (EditorGUI.EndChangeCheck())
                property.serializedObject.ApplyModifiedProperties();
            EditorGUI.EndProperty();
        }
        
        public abstract void Draw(Rect position, SerializedProperty property);
    }
}