using R3;
using S1LV3Rman.RockFall.CoreGameplay;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace S1LV3Rman.RockFall.MainMenu
{
    public class TitleUI : DefaultUIPanel
    {
        [SerializeField] private Button _start;

        public Observable<Unit> StartPress { get; private set; }

        protected override void Awake()
        {
            StartPress = _start.OnClickAsObservable();
        }
    }
}