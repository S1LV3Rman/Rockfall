using System;
using System.Collections.Generic;
using UnityEngine;

namespace S1LV3Rman.RockFall
{
    [Serializable]
    public struct KeyedItem<TKey, TValue>
    {
        [field: SerializeField] public TKey Key { get; private set; }
        [field: SerializeField] public TValue Value { get; private set; }

        public KeyedItem(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }
    }

    [Serializable]
    public class KeyedList<TKey, TValue> : ISerializationCallbackReceiver
    {
        [SerializeField] private List<KeyedItem<TKey, TValue>> _items = new();

        private Dictionary<TKey, TValue> _lookup;

        public TValue this[TKey key] => _lookup[key];

        public bool TryGetValue(TKey key, out TValue value)
            => _lookup.TryGetValue(key, out value);

        public bool ContainsKey(TKey key)
            => _lookup.ContainsKey(key);

        public void Add(TKey key, TValue value)
        {
            if (_lookup.ContainsKey(key))
                throw new ArgumentException($"Key {key} already exists.");

            _items.Add(new KeyedItem<TKey, TValue>(key, value));
            _lookup[key] = value;
        }

        public bool Remove(TKey key)
        {
            if (!_lookup.Remove(key))
                return false;

            _items.RemoveAll(i => EqualityComparer<TKey>.Default.Equals(i.Key, key));
            return true;
        }

        public void OnBeforeSerialize()
        {
            // List is already up-to-date since we edit it directly in inspector
        }

        public void OnAfterDeserialize()
        {
            _lookup = new Dictionary<TKey, TValue>(_items.Count);
            foreach (var item in _items)
                if (!_lookup.ContainsKey(item.Key))
                    _lookup[item.Key] = item.Value;
        }
    }
}