using System;
using R3;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace S1LV3Rman.RockFall.CoreGameplay
{
    public class DistanceIndicator : UIBehaviour, IDisposable
    {
        [SerializeField] private TMP_Text _distanceLabel;

        private Transform _distanceFrom;
        private Transform _distanceTo;

        private IDisposable _subscription;

        public void SetDistanceTargets(AliveTrackedMonoBehaviour from, AliveTrackedMonoBehaviour to)
        {
            _distanceFrom = from.transform;
            _distanceTo = to.transform;
            
            _subscription = Observable.Merge(from.IsAlive, to.IsAlive)
                .Where(isAlive => !isAlive)
                .Take(1)
                .Subscribe(_ => Dispose());
            
            UpdateDistance();
        }

        public void SetColor(Color color) => _distanceLabel.color = color;

        private void Update()
        {
            if (!isActiveAndEnabled)
                return;
            
            if (_distanceFrom == null || _distanceTo == null)
                return;
            
            UpdateDistance();
        }

        private void UpdateDistance()
        {
            var distance = Mathf.FloorToInt(Vector3.Distance(_distanceFrom.position, _distanceTo.position));
            _distanceLabel.text = distance + "m";
        }

        public void Dispose()
        {
            _subscription?.Dispose();
            Destroy(gameObject);
        }
    }
}