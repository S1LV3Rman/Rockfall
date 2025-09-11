using System;
using S1LV3Rman.RockFall.CoreGameplay;
using UnityEditor;
using UnityEngine;

namespace S1LV3Rman.RockFall.Editor
{
    [CustomPropertyDrawer(typeof(WeaponData))]
    public class WeaponDataDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            WeaponData temp;
            var typeProperty = property.FindPropertyRelative(nameof(temp.DamageType));

            var height = EditorGUIUtility.singleLineHeight + GUISizes.LineWithSpacing * 6; // always shown

            if (!property.IsInsideOfArray() && label != GUIContent.none)
                height += GUISizes.LineWithSpacing; // label

            var damageType = (DamageType) typeProperty.enumValueIndex;
            switch (damageType)
            {
                case DamageType.None:
                    break;
                case DamageType.Kinetic:
                    height += GUISizes.LineWithSpacing * 3;
                    break;
                case DamageType.Laser:
                    height += GUISizes.LineWithSpacing * 2;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return height;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            EditorGUI.BeginChangeCheck();

            var nested = property.IsInsideOfArray() || label == GUIContent.none;

            if (!nested)
            {
                EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
                position.y += EditorGUIUtility.singleLineHeight;
                EditorGUI.indentLevel++;
            }

            WeaponData temp;
            var prefabProperty = property.FindPropertyRelative(nameof(temp.Prefab));
            var typeProperty = property.FindPropertyRelative(nameof(temp.DamageType));
            var damageProperty = property.FindPropertyRelative(nameof(temp.Damage));
            var fireRateProperty = property.FindPropertyRelative(nameof(temp.FireRate));
            var cooldownProperty = property.FindPropertyRelative(nameof(temp.Cooldown));
            var projectileSpeedProperty = property.FindPropertyRelative(nameof(temp.ProjectileSpeed));
            var projectileLifetimeProperty = property.FindPropertyRelative(nameof(temp.ProjectileLifetime));
            var maxFireDistanceProperty = property.FindPropertyRelative(nameof(temp.MaxFireDistance));
            var muzzleFlashPrefabProperty = property.FindPropertyRelative(nameof(temp.MuzzleFlashPrefab));
            var projectilePrefabProperty = property.FindPropertyRelative(nameof(temp.ProjectilePrefab));
            var laserPrefabProperty = property.FindPropertyRelative(nameof(temp.LaserPrefab));
            var fireSoundProperty = property.FindPropertyRelative(nameof(temp.FireSound));

            prefabProperty.Draw(ref position);

            var prefab = prefabProperty.objectReferenceValue as BaseWeapon;
            typeProperty.enumValueIndex = prefab == null ? 0 : (int) prefab.DamageType;
            typeProperty.Draw(ref position, isReadonly: true);

            damageProperty.Draw(ref position);

            fireRateProperty.Draw(ref position);
            if (!Mathf.Approximately(fireRateProperty.floatValue, 1f / cooldownProperty.floatValue))
                cooldownProperty.floatValue = 1f / fireRateProperty.floatValue;
            cooldownProperty.Draw(ref position);
            if (!Mathf.Approximately(cooldownProperty.floatValue, 1f / fireRateProperty.floatValue))
                fireRateProperty.floatValue = 1f / cooldownProperty.floatValue;

            switch ((DamageType) typeProperty.enumValueIndex)
            {
                case DamageType.None:
                    break;
                case DamageType.Kinetic:
                    projectileSpeedProperty.Draw(ref position);
                    projectileLifetimeProperty.Draw(ref position);
                    break;
                case DamageType.Laser:
                    maxFireDistanceProperty.Draw(ref position);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            muzzleFlashPrefabProperty.Draw(ref position);

            switch ((DamageType) typeProperty.enumValueIndex)
            {
                case DamageType.None:
                    break;
                case DamageType.Kinetic:
                    projectilePrefabProperty.Draw(ref position);
                    break;
                case DamageType.Laser:
                    laserPrefabProperty.Draw(ref position);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            fireSoundProperty.Draw(ref position);

            if (!nested) EditorGUI.indentLevel--;

            if (EditorGUI.EndChangeCheck())
                property.serializedObject.ApplyModifiedProperties();
            EditorGUI.EndProperty();
        }
    }
}