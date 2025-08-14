using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace S1LV3Rman.RockFall.Editor
{
    [CustomPropertyDrawer(typeof(KeyedList<,>), true)]
    public class SerializableKeyedListDrawer : PropertyDrawer
    {
        private bool _foldout;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (!_foldout)
                return EditorGUIUtility.singleLineHeight;

            var itemsProp = property.FindPropertyRelative("_items");
            return EditorGUIUtility.singleLineHeight +
                   (itemsProp.isExpanded ? EditorGUI.GetPropertyHeight(itemsProp) : 0);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var itemsProp = property.FindPropertyRelative("_items");
            _foldout = EditorGUI.Foldout(
                new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight),
                _foldout, label, true);

            if (!_foldout)
                return;

            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(itemsProp, new GUIContent("Items"), true);

            // Duplicate key validation
            var keys = new HashSet<string>();
            for (int i = 0; i < itemsProp.arraySize; i++)
            {
                var element = itemsProp.GetArrayElementAtIndex(i);
                var keyProp = element.FindPropertyRelative("Key");

                if (keyProp.propertyType == SerializedPropertyType.String)
                {
                    var keyValue = keyProp.stringValue;
                    if (!string.IsNullOrEmpty(keyValue) && !keys.Add(keyValue))
                    {
                        EditorGUILayout.HelpBox($"Duplicate key \"{keyValue}\" found at index {i}.", MessageType.Error);
                    }
                }
                else
                {
                    // Non-string keys: fallback to Equals-based duplicate detection
                    object boxedKey = keyProp.boxedValue;
                    if (boxedKey != null && !keys.Add(boxedKey.ToString()))
                    {
                        EditorGUILayout.HelpBox($"Duplicate key {boxedKey} found at index {i}.", MessageType.Error);
                    }
                }
            }

            EditorGUI.indentLevel--;
        }
    }
}