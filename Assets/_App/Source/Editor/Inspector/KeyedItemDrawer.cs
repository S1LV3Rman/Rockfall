using UnityEditor;
using UnityEngine;

namespace S1LV3Rman.RockFall.Editor
{
    [CustomPropertyDrawer(typeof(KeyedItem<,>), true)]
    public class KeyedItemDrawer : PropertyDrawerLabeledOnLeft
    {
        public override void Draw(Rect position, SerializedProperty property)
        {
            var keyProp = property.FindPropertyRelative("<Key>k__BackingField");
            var valueProp = property.FindPropertyRelative("<Value>k__BackingField");

            var isComplex = valueProp.hasVisibleChildren;

            if (isComplex)
            {
                valueProp.DrawWithFoldout(ref position, GUIContent.none, GUIFlow.None);
                keyProp.DrawWithoutLabel(ref position);
            }
            else
            {
                position.width = (position.width - GUISizes.HSpacing) * 0.5f;
                keyProp.DrawWithoutLabel(ref position, GUIFlow.Horizontal);
                valueProp.DrawWithoutLabel(ref position);
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var valueProp = property.FindPropertyRelative("<Value>k__BackingField");
            var isComplex = valueProp.hasVisibleChildren;

            if (!isComplex || !valueProp.isExpanded)
                return GUISizes.SingleLine;

            return EditorGUI.GetPropertyHeight(valueProp, GUIContent.none) + GUISizes.LineWithSpacing;
        }
    }
}