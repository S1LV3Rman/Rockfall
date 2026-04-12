using UnityEditor;

namespace S1LV3Rman.RockFall
{
    public static class GUISizes
    {
        public static readonly float SingleLine = EditorGUIUtility.singleLineHeight;

        public static readonly float VSpacing = EditorGUIUtility.standardVerticalSpacing;

        public static readonly float HSpacing = 2f;

        public static readonly float LineWithSpacing = SingleLine + VSpacing;

        public static readonly float LabelWidth = EditorGUIUtility.labelWidth;
    }
}