using R3;
using UnityEngine;
using UnityEngine.UI;

namespace S1LV3Rman.RockFall.CoreGameplay
{
    public class InGameUI : DefaultUIPanel
    {
        [SerializeField] private Button _pause;
        [SerializeField] private Button _fire;

        public Observable<Unit> PausePress { get; private set; }
        public Observable<Unit> FirePress { get; private set; }

        protected override void Awake()
        {
            PausePress = _pause.OnClickAsObservable();
            FirePress = _fire.OnClickAsObservable();
        }
    }
}