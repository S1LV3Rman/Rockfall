using UnityEditor;
using UnityEngine;

namespace S1LV3Rman.RockFall.Editor
{
    [CustomPropertyDrawer(typeof(KeyedItem<,>), true)]
    public class KeyedItemDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            EditorGUI.BeginChangeCheck();

            var nested = property.IsInsideOfArray() || label == GUIContent.none;

            if (!nested) 
                position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            var keyProp = property.FindPropertyRelative("<Key>k__BackingField");
            var valueProp = property.FindPropertyRelative("<Value>k__BackingField");

            var isComplex = valueProp.hasVisibleChildren;

            if (isComplex)
            {
                keyProp.DrawWithoutLabel(ref position);
                valueProp.DrawWithFoldout(ref position, preferredLabel);
            }
            else
            {
                position.width *= 0.5f;
                position.width -= 2f;
                keyProp.DrawWithoutLabel(ref position, GUIFlow.Horizontal);
                
                position.x += 4f;
                valueProp.DrawWithoutLabel(ref position);
            }

            if (EditorGUI.EndChangeCheck())
                property.serializedObject.ApplyModifiedProperties();
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var valueProp = property.FindPropertyRelative("<Value>k__BackingField");
            var isComplex = valueProp.hasVisibleChildren;

            var height = GUISizes.LineWithSpacing;
            if (isComplex)
            {
                if (valueProp.isExpanded)
                    height += GUISizes.LineWithSpacing + EditorGUI.GetPropertyHeight(valueProp, GUIContent.none);
                else
                    height += GUISizes.SingleLine;
            }
            else
            {
                height += GUISizes.SingleLine;
            }

            return height;
        }
    }

}