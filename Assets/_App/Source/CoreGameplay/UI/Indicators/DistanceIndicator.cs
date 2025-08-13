using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace S1LV3Rman.RockFall.CoreGameplay
{
    public class DistanceIndicator : UIBehaviour
    {
        [SerializeField] private TMP_Text _distanceLabel;

        private Transform _distanceFrom;
        private Transform _distanceTo;

        public void SetDistanceTargets(Transform from, Transform to)
        {
            _distanceFrom = from;
            _distanceTo = to;
            UpdateDistance();
        }

        public void SetColor(Color color) => _distanceLabel.color = color;

        private void Update()
        {
            if (_distanceFrom == null || _distanceTo == null)
                return;
            
            UpdateDistance();
        }

        private void UpdateDistance()
        {
            var distance = Mathf.FloorToInt(Vector3.Distance(_distanceFrom.position, _distanceTo.position));
            _distanceLabel.text = distance + "m";
        }
    }
}