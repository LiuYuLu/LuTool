using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace LuTool
{
    public class BlockingDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        private readonly IDictionary<TKey, TValue> _dict;
        private readonly ReaderWriterLockSlim _locker;

        public ICollection<TKey> Keys
        {
            get
            {
                _locker.EnterReadLock();
                var list = _dict.Keys.ToList();
                _locker.ExitReadLock();
                return list;
            }
        }

        public ICollection<TValue> Values
        {
            get
            {
                _locker.EnterReadLock();
                var list = _dict.Values.ToList();
                _locker.ExitReadLock();
                return list;
            }
        }

        public TValue this[TKey key]
        {
            get
            {
                _locker.EnterReadLock();
                try
                {
                    return _dict[key];
                }
                finally
                {
                    _locker.ExitReadLock();
                }
            }
            set
            {
                _locker.EnterWriteLock();
                try
                {
                    _dict[key] = value;
                }
                finally
                {
                    _locker.ExitWriteLock();
                }
            }
        }

        public int Count
        {
            get
            {
                _locker.EnterReadLock();
                var count = _dict.Count;
                _locker.ExitReadLock();
                return count;
            }
        }

        public BlockingDictionary()
        {
            _dict = new Dictionary<TKey, TValue>();
            _locker = new ReaderWriterLockSlim();
        }

        public BlockingDictionary(int capacity)
        {
            _dict = new Dictionary<TKey, TValue>(capacity);
            _locker = new ReaderWriterLockSlim();
        }

        public void Add(TKey key, TValue value)
        {
            _locker.EnterWriteLock();
            try
            {
                _dict.Add(key, value);
            }
            finally
            {
                _locker.ExitWriteLock();
            }
        }

        public bool ContainsKey(TKey key)
        {
            _locker.EnterReadLock();
            try
            {
                return _dict.ContainsKey(key);
            }
            finally
            {
                _locker.ExitReadLock();
            }
        }

        public bool Remove(TKey key)
        {
            _locker.EnterWriteLock();
            try
            {
                return _dict.Remove(key);
            }
            finally
            {
                _locker.ExitWriteLock();
            }
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            _locker.EnterReadLock();
            try
            {
                return _dict.TryGetValue(key, out value);
            }
            finally
            {
                _locker.ExitReadLock();
            }
        }

        public TValue GetOrAdd(TKey key, TValue value)
        {
            TValue local;
            if (TryGetValue(key, out local))
                return local;

            return GetOrAddInternal(key, _ => value);
        }

        private TValue GetOrAddInternal(TKey key, Func<TKey, TValue> valueFactory)
        {
            _locker.EnterUpgradeableReadLock();
            try
            {
                TValue local;
                if (_dict.TryGetValue(key, out local))
                    return local;

                var value = valueFactory(key);
                _locker.EnterWriteLock();
                try
                {
                    _dict.Add(key, value);
                    return value;
                }
                finally
                {
                    _locker.ExitWriteLock();
                }
            }
            finally
            {
                _locker.ExitUpgradeableReadLock();
            }
        }

        public TValue GetOrAdd(TKey key, Func<TKey, TValue> valueFactory)
        {
            TValue local;
            if (TryGetValue(key, out local))
                return local;

            return GetOrAddInternal(key, valueFactory);
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            _locker.EnterReadLock();
            using (var enumerator = _dict.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    yield return enumerator.Current;
                }
            }
            _locker.ExitReadLock();
        }

        public void Clear()
        {
            _locker.EnterWriteLock();
            try
            {
                _dict.Clear();
            }
            finally
            {
                _locker.ExitWriteLock();
            }
        }

        #region Implement Interface

        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
        {
            _locker.EnterWriteLock();
            try
            {
                _dict.Add(item);
            }
            finally
            {
                _locker.ExitWriteLock();
            }
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
        {
            _locker.EnterReadLock();
            try
            {
                return _dict.Contains(item);
            }
            finally
            {
                _locker.ExitReadLock();
            }
        }

        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int index)
        {
            _locker.EnterReadLock();
            try
            {
                _dict.CopyTo(array, index);
            }
            finally
            {
                _locker.ExitReadLock();
            }
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
        {
            _locker.EnterWriteLock();
            try
            {
                return _dict.Remove(item);
            }
            finally
            {
                _locker.ExitWriteLock();
            }
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly => false;

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

    }
}
