using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace S1LV3Rman.RockFall.Editor
{
    [CustomPropertyDrawer(typeof(ShowIfAttribute))]
    public class ShowIfDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var showIf = (ShowIfAttribute)attribute;
            bool enabled = EvaluateCondition(property, showIf);

            // Return normal height if visible, 0 if hidden
            return enabled ? EditorGUI.GetPropertyHeight(property, label, true) : 0f;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var showIf = (ShowIfAttribute)attribute;
            bool enabled = EvaluateCondition(property, showIf);

            if (enabled)
                EditorGUI.PropertyField(position, property, label, true);
        }

        private bool EvaluateCondition(SerializedProperty property, ShowIfAttribute showIf)
        {
            // target object
            object target = property.serializedObject.targetObject;
            var t = target.GetType();

            // look for field/property/method
            BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            var member = t.GetField(showIf.ConditionName, flags);
            if (member != null && member.FieldType == typeof(bool))
                return (bool)member.GetValue(target) ^ showIf.Invert;

            var prop = t.GetProperty(showIf.ConditionName, flags);
            if (prop != null && prop.PropertyType == typeof(bool))
                return (bool)prop.GetValue(target) ^ showIf.Invert;

            var method = t.GetMethod(showIf.ConditionName, flags);
            if (method != null && method.ReturnType == typeof(bool) && method.GetParameters().Length == 0)
                return (bool)method.Invoke(target, null) ^ showIf.Invert;

            Debug.LogWarning($"ShowIf: Could not find bool field/property/method '{showIf.ConditionName}' on {t}");
            return true; // default show
        }
    }
}