using Eflatun.SceneReference;
using UnityEngine;

namespace S1LV3Rman.RockFall
{
    [CreateAssetMenu(fileName = nameof(AppScenesConfig), menuName = "Config/" + nameof(AppScenesConfig), order = 0)]
    public class AppScenesConfig : ScriptableObject
    {
        [field: SerializeField] public SceneReference EmptyScene { get; private set; }
        [field: SerializeField] public SceneReference MainMenu { get; private set; }
        [field: SerializeField] public SceneReference CoreGameplay { get; private set; }
    }
}