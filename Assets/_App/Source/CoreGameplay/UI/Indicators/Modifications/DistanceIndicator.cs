using R3;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace S1LV3Rman.RockFall.CoreGameplay
{
    public class DistanceIndicator : UIBehaviour, IIndicatorModification
    {
        [SerializeField] private TMP_Text _distanceLabel;

        private readonly SerialDisposable _anyTargetLostSub = new();
        private readonly Subject<DistanceIndicator> _anyTargetLost = new();
        public Observable<DistanceIndicator> AnyTargetLost => _anyTargetLost;

        public AliveTrackedMonoBehaviour FromTarget { get; private set; }
        public AliveTrackedMonoBehaviour ToTarget { get; private set; }

        public void SetDistanceTargets(AliveTrackedMonoBehaviour fromTarget, AliveTrackedMonoBehaviour toTarget)
        {
            FromTarget = fromTarget;
            ToTarget = toTarget;

            _anyTargetLostSub.Dispose();
            _anyTargetLostSub.Disposable = Observable.Merge(fromTarget.IsAlive, toTarget.IsAlive)
                .Where(isAlive => !isAlive)
                .Take(1)
                .Subscribe(_ =>
                {
                    FromTarget = null;
                    ToTarget = null;
                    _anyTargetLost.OnNext(this);
                });
        }

        public void SetColor(Color color) => _distanceLabel.color = color;

        public void AttachToIndicator(Indicator indicator)
        {
            UpdateDistance();
        }

        private void Update()
        {
            if (!isActiveAndEnabled)
                return;

            if (FromTarget == null || ToTarget == null)
                return;

            UpdateDistance();
        }

        private void UpdateDistance()
        {
            var distance =
                Mathf.FloorToInt(Vector3.Distance(FromTarget.transform.position, ToTarget.transform.position));
            _distanceLabel.text = distance + "m";
        }

        public void Remove()
        {
            _anyTargetLostSub.Dispose();
            _anyTargetLost.OnCompleted(Result.Success);
            Destroy(gameObject);
        }
    }
}