using System;
using System.Collections;
using System.Collections.Generic;
using R3;
using UnityEngine;

namespace S1LV3Rman.RockFall
{
    public class DisposableList<T> : IList<T>, IDisposable where T : IDisposable
    {
        protected readonly List<T> _items = new();
        protected bool _isDisposed;

        private readonly Subject<T> _onAdded = new();
        private readonly Subject<T> _onRemoved = new();

        public Observable<T> OnAdded => _onAdded;
        public Observable<T> OnRemoved => _onRemoved;

        public T this[int index]
        {
            get => _items[index];
            set
            {
                ThrowIfDisposed();
                if (value == null) throw new ArgumentNullException(nameof(value));

                var oldItem = _items[index];
                if (ReferenceEquals(oldItem, value))
                    return;

                _items[index] = value;
                DisposeItem(oldItem);
                _onRemoved.OnNext(oldItem);
                _onAdded.OnNext(value);
            }
        }

        public int Count => _items.Count;
        public bool IsReadOnly => false;

        public virtual void Add(T item)
        {
            ThrowIfDisposed();
            if (item == null) throw new ArgumentNullException(nameof(item));
            _items.Add(item);
            _onAdded.OnNext(item);
        }

        public virtual bool Remove(T item)
        {
            ThrowIfDisposed();
            if (item == null) return false;
            var removed = _items.Remove(item);
            if (removed)
            {
                DisposeItem(item);
                _onRemoved.OnNext(item);
            }

            return removed;
        }

        public void Clear()
        {
            ThrowIfDisposed();
            foreach (var item in _items)
            {
                DisposeItem(item);
                _onRemoved.OnNext(item);
            }

            _items.Clear();
        }

        public bool Contains(T item) => _items.Contains(item);
        public void CopyTo(T[] array, int arrayIndex) => _items.CopyTo(array, arrayIndex);
        public IEnumerator<T> GetEnumerator() => _items.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => _items.GetEnumerator();
        public int IndexOf(T item) => _items.IndexOf(item);

        public void Insert(int index, T item)
        {
            ThrowIfDisposed();
            if (item == null) throw new ArgumentNullException(nameof(item));
            _items.Insert(index, item);
            _onAdded.OnNext(item);
        }

        public void RemoveAt(int index)
        {
            ThrowIfDisposed();
            var item = _items[index];
            _items.RemoveAt(index);
            DisposeItem(item);
            _onRemoved.OnNext(item);
        }

        private void DisposeItem(T item)
        {
            try
            {
                item.Dispose();
            }
            catch (Exception ex)
            {
                Debug.LogError($"Dispose failed: {ex}");
            }
        }

        private void ThrowIfDisposed()
        {
            if (_isDisposed) throw new ObjectDisposedException(nameof(DisposableList<T>));
        }

        public virtual void Dispose()
        {
            if (_isDisposed) return;
            Clear();
            _onAdded.OnCompleted();
            _onRemoved.OnCompleted();
            _onAdded.Dispose();
            _onRemoved.Dispose();
            _isDisposed = true;
        }
    }
}