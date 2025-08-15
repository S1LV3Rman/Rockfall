using System;
using System.Collections.Generic;
using R3;

namespace S1LV3Rman.RockFall.CoreGameplay
{
    public sealed class DamageNumbersPool : BasePool<DamageNumber>
    {
        private readonly List<DamageNumber> _hidden = new();

        private readonly IDisposable _subscriptions;

        public DamageNumbersPool()
        {
            var transfer = OnAdded.SelectMany(damageNumber =>
                    damageNumber.IsShown
                        .TakeUntil(OnRemoved.Where(r => ReferenceEquals(r, damageNumber)))
                        .Select(_ => damageNumber))
                .Subscribe(TransferDamageNumber);
            var removal = OnRemoved.Subscribe(damageNumber => _hidden.Remove(damageNumber));
            _subscriptions = Disposable.Combine(transfer, removal);
        }

        private void TransferDamageNumber(DamageNumber damageNumber)
        {
            if (damageNumber.IsShown.CurrentValue)
                _hidden.Remove(damageNumber);
            else
                _hidden.Add(damageNumber);
        }

        public bool TryGetDamageNumber(out DamageNumber damageNumber)
        {
            if (_hidden.Count <= 0)
            {
                damageNumber = null;
                return false;
            }

            damageNumber = _hidden[^1];
            return true;
        }

        public override void Dispose()
        {
            base.Dispose();
            _subscriptions.Dispose();
        }
    }
}