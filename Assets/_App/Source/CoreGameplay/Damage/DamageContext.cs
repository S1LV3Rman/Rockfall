using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace S1LV3Rman.RockFall.CoreGameplay
{
    public readonly struct DamageContext
    {
        public readonly IInstigator Source; // who caused it
        public readonly IDamageDealer Dealer; // what deals damage
        public readonly IDamageable Receiver; // who takes damage
        public readonly Vector3 HitPoint; // for numbers/VFX
        public readonly int BaseDamage; // raw damage
        public readonly DamageType DamageType; // type of damage
        public readonly bool IsCritical; // example flag
        public readonly int TeamId; // for FF checks
        public readonly string[] Tags; // for unique effects

        public DamageContext(
            IInstigator source,
            IDamageDealer dealer,
            IDamageable receiver,
            Vector3 hit,
            int baseDamage,
            DamageType damageType,
            bool isCritical = false,
            int teamId = -1,
            IEnumerable<string> tags = null)
        {
            Source = source;
            Dealer = dealer;
            Receiver = receiver;
            HitPoint = hit;
            BaseDamage = baseDamage;
            DamageType = damageType;
            IsCritical = isCritical;
            TeamId = teamId;
            Tags = tags?.ToArray() ?? Array.Empty<string>();
        }
    }
}