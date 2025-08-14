using UnityEngine;

namespace S1LV3Rman.RockFall.CoreGameplay
{
    [CreateAssetMenu(fileName = nameof(WeaponsConfig), menuName = "Config/" + nameof(WeaponsConfig), order = 0)]
    public class WeaponsConfig : ScriptableObject
    {
        [field: SerializeField] public KeyedList<WeaponType, BaseWeapon> Weapons { get; private set; }
    }

    // [Serializable]
    // public struct WeaponData
    // {
    //     public WeaponType Type;
    //     public BaseWeapon Prefab;
    // }
}