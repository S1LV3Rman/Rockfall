using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace S1LV3Rman.RockFall.Editor
{
    [CustomPropertyDrawer(typeof(KeyedList<,>), true)]
    public class KeyedListDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            EditorGUI.BeginChangeCheck();

            var itemsProp = property.FindPropertyRelative("_items");

            itemsProp.isExpanded = true;
            itemsProp.Draw(ref position, label);

            // Duplicate key validation
            var keys = new HashSet<string>();
            for (var i = 0; i < itemsProp.arraySize; i++)
            {
                var element = itemsProp.GetArrayElementAtIndex(i);
                var keyProp = element.FindPropertyRelative("<Key>k__BackingField");
            
                if (keyProp.propertyType == SerializedPropertyType.String)
                {
                    var keyValue = keyProp.stringValue;
                    if (!string.IsNullOrEmpty(keyValue) && !keys.Add(keyValue))
                        EditorGUILayout.HelpBox($"Duplicate key \"{keyValue}\" found at index {i}.",
                            MessageType.Error);
                }
                else
                {
                    // Non-string keys: fallback to Equals-based duplicate detection
                    var boxedKey = keyProp.boxedValue;
                    if (boxedKey != null && !keys.Add(boxedKey.ToString()))
                        EditorGUILayout.HelpBox($"Duplicate key {boxedKey} found at index {i}.",
                            MessageType.Error);
                }
            }

            if (EditorGUI.EndChangeCheck())
                property.serializedObject.ApplyModifiedProperties();
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var itemsProp = property.FindPropertyRelative("_items");
            itemsProp.isExpanded = true;
            return EditorGUI.GetPropertyHeight(itemsProp);
        }
    }
}