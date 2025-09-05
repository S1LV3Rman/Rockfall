using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace S1LV3Rman.RockFall.CoreGameplay
{
    public sealed class DamageProcessor
    {
        private readonly List<IPreDamageModifier> _pre;
        private readonly List<IMidDamageModifier> _mid;
        private readonly List<IPostDamageModifier> _post;

        public DamageProcessor(
            IEnumerable<IPreDamageModifier> pre,
            IEnumerable<IMidDamageModifier> mid,
            IEnumerable<IPostDamageModifier> post)
        {
            _pre = pre.ToList();
            _mid = mid.ToList();
            _post = post.ToList();
        }

        public int Compute(ref DamageContext ctx, int raw)
        {
            foreach (var m in _pre) raw = m.PreModify(ref ctx, raw);
            foreach (var m in _mid) raw = m.MidModify(ref ctx, raw);
            foreach (var m in _post) raw = m.PostModify(ref ctx, raw);
            var applied = Mathf.Max(0, raw);
            return applied;
        }
    }
}