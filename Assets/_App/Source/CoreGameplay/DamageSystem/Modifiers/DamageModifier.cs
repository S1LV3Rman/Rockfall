using System;
using System.Collections.Generic;
using UnityEngine;

namespace S1LV3Rman.RockFall.CoreGameplay
{
    public sealed class DamageModifier : IDisposable
    {
        private readonly List<IDamageModifier> _pre = new();
        private readonly List<IDamageModifier> _mid = new();
        private readonly List<IDamageModifier> _post = new();

        public void AddPreModifier(IDamageModifier modifier) => _pre.Add(modifier);

        public void AddMidModifier(IDamageModifier modifier) => _mid.Add(modifier);

        public void AddPostModifier(IDamageModifier modifier) => _post.Add(modifier);

        public int Modify(ref DamageContext context, int incoming)
        {
            foreach (var m in _pre) incoming = m.Modify(ref context, incoming);
            foreach (var m in _mid) incoming = m.Modify(ref context, incoming);
            foreach (var m in _post) incoming = m.Modify(ref context, incoming);

            return Mathf.Max(0, Mathf.RoundToInt(incoming));
        }

        public void OnApplied(DamageContext context, int applied)
        {
            foreach (var m in _pre) m.OnApplied(context, applied);
            foreach (var m in _mid) m.OnApplied(context, applied);
            foreach (var m in _post) m.OnApplied(context, applied);
        }

        public void Dispose()
        {
            _pre.Clear();
            _mid.Clear();
            _post.Clear();
        }
    }
}