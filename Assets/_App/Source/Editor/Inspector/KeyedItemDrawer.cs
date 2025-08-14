using UnityEditor;
using UnityEngine;

namespace S1LV3Rman.RockFall.Editor
{
    [CustomPropertyDrawer(typeof(KeyedItem<,>), true)]
    public class KeyedItemDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var keyProp = property.FindPropertyRelative("Key");
            var valueProp = property.FindPropertyRelative("Value");

            var halfWidth = position.width / 2f;
            var keyRect = new Rect(position.x, position.y, halfWidth - 2, position.height);
            var valueRect = new Rect(position.x + halfWidth + 2, position.y, halfWidth - 2, position.height);

            EditorGUI.PropertyField(keyRect, keyProp, GUIContent.none);
            EditorGUI.PropertyField(valueRect, valueProp, GUIContent.none);
        }
    }

}