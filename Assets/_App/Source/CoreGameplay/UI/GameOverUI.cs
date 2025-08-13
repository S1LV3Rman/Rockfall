using R3;
using UnityEngine;
using UnityEngine.UI;

namespace S1LV3Rman.RockFall.CoreGameplay
{
    public class GameOverUI : DefaultUIPanel
    {
        [SerializeField] private Button _retry;

        public Observable<Unit> RetryPress { get; private set; }

        protected override void Awake()
        {
            RetryPress = _retry.OnClickAsObservable();
        }
    }
}