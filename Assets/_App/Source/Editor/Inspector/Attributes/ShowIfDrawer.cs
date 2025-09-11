using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace S1LV3Rman.RockFall.Editor
{
    [CustomPropertyDrawer(typeof(ShowIfAttribute))]
    public class ShowIfDrawer : PropertyDrawer
    {
        private const BindingFlags FLAGS = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var showIf = (ShowIfAttribute) attribute;
            return EvaluateCondition(property, showIf)
                ? EditorGUI.GetPropertyHeight(property, label, true)
                : 0f;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var showIf = (ShowIfAttribute) attribute;
            if (EvaluateCondition(property, showIf))
                EditorGUI.PropertyField(position, property, label, true);
        }

        private bool EvaluateCondition(SerializedProperty property, ShowIfAttribute showIf)
        {
            object target = property.serializedObject.targetObject;
            var t = target.GetType();

            // Field / property / method lookup
            var field = t.GetField(showIf.ConditionName, FLAGS);
            var prop = t.GetProperty(showIf.ConditionName, FLAGS);
            var method = t.GetMethod(showIf.ConditionName, FLAGS);

            object value = null;
            if (field != null) value = field.GetValue(target);
            else if (prop != null) value = prop.GetValue(target);
            else if (method != null && method.GetParameters().Length == 0) value = method.Invoke(target, null);

            if (value == null)
            {
                Debug.LogWarning($"ShowIf: Cannot find member '{showIf.ConditionName}' on {t}");
                return true; // default show
            }

            bool result;

            if (showIf.CompareValue == null)
            {
                // Boolean check
                if (value is bool b) result = b;
                else
                {
                    Debug.LogWarning(
                        $"ShowIf: Member '{showIf.ConditionName}' is not a bool but no compareValue supplied.");
                    result = true;
                }
            }
            else
            {
                // Compare value — handle enums and primitives
                var compareVal = showIf.CompareValue;
                if (value.GetType().IsEnum)
                {
                    if (compareVal is string s)
                        compareVal = System.Enum.Parse(value.GetType(), s);
                    else if (compareVal.GetType() != value.GetType())
                        compareVal =
                            System.Convert.ChangeType(compareVal, System.Enum.GetUnderlyingType(value.GetType()));
                }

                result = Equals(value, compareVal);
            }

            return result ^ showIf.Invert;
        }
    }
}