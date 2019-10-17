using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace Cic.OpenOne.Common.Util.Collection
{
    /// <summary>
    /// IThreadSafeDictionary
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public interface IThreadSafeDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        /// <summary>
        /// Merge is similar to the SQL merge or upsert statement.  
        /// </summary>
        /// <param name="key">Key to lookup</param>
        /// <param name="newValue">New Value</param>
        void MergeSafe(TKey key, TValue newValue);

        /// <summary>
        /// This is a blind remove. Prevents the need to check for existence first.
        /// </summary>
        /// <param name="key">Key to Remove</param>
        void RemoveSafe(TKey key);
    }

    /// <summary>
    /// ThreadSafeDictionary-Klasse
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    [Serializable]
    public class ThreadSafeDictionary<TKey, TValue> : IThreadSafeDictionary<TKey, TValue>
    {
        //This is the internal dictionary that we are wrapping
        IDictionary<TKey, TValue> dict = new Dictionary<TKey, TValue>();

        [NonSerialized]
        ReaderWriterLockSlim dictionaryLock = Locks.GetLockInstance(LockRecursionPolicy.NoRecursion); //setup the lock;

        /// <summary>
        /// This is a blind remove. Prevents the need to check for existence first.
        /// </summary>
        /// <param name="key">Key to remove</param>
        public void RemoveSafe(TKey key)
        {
            using (new ReadLock(this.dictionaryLock))
            {
                if (this.dict.ContainsKey(key))
                {
                    using (new WriteLock(this.dictionaryLock))
                    {
                        this.dict.Remove(key);
                    }
                }
            }
        }

        /// <summary>
        /// Merge does a blind remove, and then add.  Basically a blind Upsert.  
        /// </summary>
        /// <param name="key">Key to lookup</param>
        /// <param name="newValue">New Value</param>
        public virtual void MergeSafe(TKey key, TValue newValue)
        {
            using (new WriteLock(this.dictionaryLock)) // take a writelock immediately since we will always be writing
            {
                if (this.dict.ContainsKey(key))
                {
                    this.dict.Remove(key);
                }
                this.dict.Add(key, newValue);
            }
        }

        /// <summary>
        /// Remove
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual bool Remove(TKey key)
        {
            using (new WriteLock(this.dictionaryLock))
            {
                return this.dict.Remove(key);
            }
        }

        /// <summary>
        /// ContainsKey
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual bool ContainsKey(TKey key)
        {
            using (new ReadOnlyLock(this.dictionaryLock))
            {
                return this.dict.ContainsKey(key);
            }
        }

        /// <summary>
        /// TryGetValue
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual bool TryGetValue(TKey key, out TValue value)
        {
            using (new ReadOnlyLock(this.dictionaryLock))
            {
                return this.dict.TryGetValue(key, out value);
            }
        }

        /// <summary>
        /// this
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual TValue this[TKey key]
        {
            get
            {
                using (new ReadOnlyLock(this.dictionaryLock))
                {
                    return this.dict[key];
                }
            }
            set
            {
                using (new WriteLock(this.dictionaryLock))
                {
                    this.dict[key] = value;
                }
            }
        }

        /// <summary>
        /// Keys-Collection
        /// </summary>
        public virtual ICollection<TKey> Keys
        {
            get
            {
                using (new ReadOnlyLock(this.dictionaryLock))
                {
                    return new List<TKey>(this.dict.Keys);
                }
            }
        }

        /// <summary>
        /// Values-Collection
        /// </summary>
        public virtual ICollection<TValue> Values
        {
            get
            {
                using (new ReadOnlyLock(this.dictionaryLock))
                {
                    return new List<TValue>(this.dict.Values);
                }
            }
        }

        /// <summary>
        /// Clear
        /// </summary>
        public virtual void Clear()
        {
            using (new WriteLock(this.dictionaryLock))
            {
                this.dict.Clear();
            }
        }

        /// <summary>
        /// Count
        /// </summary>
        public virtual int Count
        {
            get
            {
                using (new ReadOnlyLock(this.dictionaryLock))
                {
                    return this.dict.Count;
                }
            }
        }

        /// <summary>
        /// Contains
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public virtual bool Contains(KeyValuePair<TKey, TValue> item)
        {
            using (new ReadOnlyLock(this.dictionaryLock))
            {
                return this.dict.Contains(item);
            }
        }

        /// <summary>
        /// Add
        /// </summary>
        /// <param name="item"></param>
        public virtual void Add(KeyValuePair<TKey, TValue> item)
        {
            using (new WriteLock(this.dictionaryLock))
            {
                this.dict.Add(item);
            }
        }

        /// <summary>
        /// Add
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public virtual void Add(TKey key, TValue value)
        {
            if (ContainsKey(key)) return;
            using (new WriteLock(this.dictionaryLock))
            {
                this.dict.Add(key, value);
            }
        }

        /// <summary>
        /// Remove
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public virtual bool Remove(KeyValuePair<TKey, TValue> item)
        {
            using (new WriteLock(this.dictionaryLock))
            {
                return this.dict.Remove(item);
            }
        }

        /// <summary>
        /// CopyTo
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        public virtual void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            using (new ReadOnlyLock(this.dictionaryLock))
            {
                this.dict.CopyTo(array, arrayIndex);
            }
        }

        /// <summary>
        /// IsReadOnly
        /// </summary>
        public virtual bool IsReadOnly
        {
            get
            {
                using (new ReadOnlyLock(this.dictionaryLock))
                {
                    return this.dict.IsReadOnly;
                }
            }
        }

        /// <summary>
        /// GetEnumerator
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            throw new NotSupportedException("Cannot enumerate a threadsafe dictionary.  Instead, enumerate the keys or values collection.");
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotSupportedException("Cannot enumerate a threadsafe dictionary.  Instead, enumerate the keys or values collection");
        }
    }

    /// <summary>
    /// Locks-Klasse static
    /// </summary>
    public static class Locks
    {
        /// <summary>
        /// GetReadLock
        /// </summary>
        /// <param name="locks"></param>
        public static void GetReadLock(ReaderWriterLockSlim locks)
        {
            bool lockAcquired = false;
            while (!lockAcquired)
                lockAcquired = locks.TryEnterUpgradeableReadLock(1);
        }

        /// <summary>
        /// GetReadOnlyLock
        /// </summary>
        /// <param name="locks"></param>
        public static void GetReadOnlyLock(ReaderWriterLockSlim locks)
        {
            bool lockAcquired = false;
            while (!lockAcquired)
                lockAcquired = locks.TryEnterReadLock(1);
        }

        /// <summary>
        /// GetWriteLock
        /// </summary>
        /// <param name="locks"></param>
        public static void GetWriteLock(ReaderWriterLockSlim locks)
        {
            bool lockAcquired = false;
            while (!lockAcquired)
                lockAcquired = locks.TryEnterWriteLock(1);
        }

        /// <summary>
        /// ReleaseReadOnlyLock
        /// </summary>
        /// <param name="locks"></param>
        public static void ReleaseReadOnlyLock(ReaderWriterLockSlim locks)
        {
            if (locks.IsReadLockHeld)
                locks.ExitReadLock();
        }

        /// <summary>
        /// ReleaseReadLock
        /// </summary>
        /// <param name="locks"></param>
        public static void ReleaseReadLock(ReaderWriterLockSlim locks)
        {
            if (locks.IsUpgradeableReadLockHeld)
                locks.ExitUpgradeableReadLock();
        }

        /// <summary>
        /// ReleaseWriteLock
        /// </summary>
        /// <param name="locks"></param>
        public static void ReleaseWriteLock(ReaderWriterLockSlim locks)
        {
            if (locks.IsWriteLockHeld)
                locks.ExitWriteLock();
        }

        /// <summary>
        /// ReleaseLock
        /// </summary>
        /// <param name="locks"></param>
        public static void ReleaseLock(ReaderWriterLockSlim locks)
        {
            ReleaseWriteLock(locks);
            ReleaseReadLock(locks);
            ReleaseReadOnlyLock(locks);
        }

        /// <summary>
        /// GetLockInstance
        /// </summary>
        /// <returns></returns>
        public static ReaderWriterLockSlim GetLockInstance()
        {
            return GetLockInstance(LockRecursionPolicy.SupportsRecursion);
        }

        /// <summary>
        /// GetLockInstance
        /// </summary>
        /// <param name="recursionPolicy"></param>
        /// <returns></returns>
        public static ReaderWriterLockSlim GetLockInstance(LockRecursionPolicy recursionPolicy)
        {
            return new ReaderWriterLockSlim(recursionPolicy);
        }
    }

    /// <summary>
    /// BaseLock
    /// </summary>
    public abstract class BaseLock : IDisposable
    {
        /// <summary>
        /// pLocks
        /// </summary>
        protected ReaderWriterLockSlim pLocks;

        /// <summary>
        /// BaseLock
        /// </summary>
        /// <param name="locks"></param>
        public BaseLock(ReaderWriterLockSlim locks)
        {
            pLocks = locks;
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public abstract void Dispose();
    }

    /// <summary>
    /// ReadLock
    /// </summary>
    public class ReadLock : BaseLock
    {
        /// <summary>
        /// ReadLock
        /// </summary>
        /// <param name="locks"></param>
        public ReadLock(ReaderWriterLockSlim locks)
            : base(locks)
        {
            Locks.GetReadLock(this.pLocks);
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public override void Dispose()
        {
            Locks.ReleaseReadLock(this.pLocks);
        }
    }

    /// <summary>
    /// ReadOnlyLock-Klasse
    /// </summary>
    public class ReadOnlyLock : BaseLock
    {
        /// <summary>
        /// ReadOnlyLock
        /// </summary>
        /// <param name="locks"></param>
        public ReadOnlyLock(ReaderWriterLockSlim locks)
            : base(locks)
        {
            Locks.GetReadOnlyLock(this.pLocks);
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public override void Dispose()
        {
            Locks.ReleaseReadOnlyLock(this.pLocks);
        }
    }

    /// <summary>
    /// WriteLock-Klasse
    /// </summary>
    public class WriteLock : BaseLock
    {
        /// <summary>
        /// WriteLock
        /// </summary>
        /// <param name="locks"></param>
        public WriteLock(ReaderWriterLockSlim locks)
            : base(locks)
        {
            Locks.GetWriteLock(this.pLocks);
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public override void Dispose()
        {
            Locks.ReleaseWriteLock(this.pLocks);
        }
    }
}