using R3;
using UnityEngine;
using UnityEngine.UI;

namespace S1LV3Rman.RockFall.CoreGameplay
{
    public class PauseUI : DefaultUIPanel
    {
        [SerializeField] private Button _resume;
        [SerializeField] private Button _toMainMenu;

        public Observable<Unit> ResumePress { get; private set; }
        public Observable<Unit> ToMainMenuPress { get; private set; }

        protected override void Awake()
        {
            ResumePress = _resume.OnClickAsObservable();
            ToMainMenuPress = _toMainMenu.OnClickAsObservable();
        }
    }
}