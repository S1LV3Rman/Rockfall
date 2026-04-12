using System;
using System.Collections.Generic;

namespace S1LV3Rman.RockFall.CoreGameplay
{
    public sealed class AttackModifier : IDisposable
    {
        private readonly List<IAttackModifier> _pre = new();
        private readonly List<IAttackModifier> _mid = new();
        private readonly List<IAttackModifier> _post = new();

        public void AddPreModifier(IAttackModifier modifier) => _pre.Add(modifier);

        public void AddMidModifier(IAttackModifier modifier) => _mid.Add(modifier);

        public void AddPostModifier(IAttackModifier modifier) => _post.Add(modifier);

        public void Modify(ref AttackContext context)
        {
            foreach (var m in _pre) m.Modify(ref context);
            foreach (var m in _mid) m.Modify(ref context);
            foreach (var m in _post) m.Modify(ref context);
        }

        public void Dispose()
        {
            _pre.Clear();
            _mid.Clear();
            _post.Clear();
        }
    }
}