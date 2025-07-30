using System.Collections.Generic;
using UnityEngine;

namespace Scripts
{
    public class DamageNumbersManager : Singleton<DamageNumbersManager>
    {
        [SerializeField] private Camera _mainCamera;
        [SerializeField] private RectTransform _uiContainer;
        [SerializeField] private DamageNumber _damageNumberPrefab;

        private readonly List<DamageNumber> _shownDamageNumbers = new();
        private readonly List<DamageNumber> _hiddenDamageNumbers = new();

        public void Create(Vector3 position, int amount)
        {
            var damageNumber = GetDamageNumber();
            damageNumber.Show(position, amount, _mainCamera);
            damageNumber.SetPositionAt(0f);
            _shownDamageNumbers.Add(damageNumber);
        }

        private DamageNumber GetDamageNumber()
        {
            if (_hiddenDamageNumbers.Count <= 0)
                return Instantiate(_damageNumberPrefab, _uiContainer);

            var damageNumber = _hiddenDamageNumbers[0];
            _hiddenDamageNumbers.RemoveAt(0);
            return damageNumber;
        }

        public void RemoveAllNumbers()
        {
            foreach (var damageNumber in _shownDamageNumbers)
                Destroy(damageNumber.gameObject);
            _shownDamageNumbers.Clear();
            
            foreach (var damageNumber in _hiddenDamageNumbers)
                Destroy(damageNumber.gameObject);
            _hiddenDamageNumbers.Clear();
        }

        private void Update()
        {
            for (var i = 0; i < _shownDamageNumbers.Count; ++i)
            {
                var damageNumber = _shownDamageNumbers[i];
                if (damageNumber.IsAlive)
                    continue;

                damageNumber.Hide();
                _hiddenDamageNumbers.Add(damageNumber);
                _shownDamageNumbers.RemoveAt(i);
                --i;
            }
        }
    }
}