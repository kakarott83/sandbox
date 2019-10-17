using System;

namespace Cic.OpenOne.Common.Util.Collection
{
    /// <summary>
    /// Dictionary with expiration time
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class CacheDictionary<TKey, TValue> : ThreadSafeDictionary<TKey, TValue>
    {
        private long duration = 0;
        private ThreadSafeDictionary<TKey, double> durations = new ThreadSafeDictionary<TKey, double>();

        /// <summary>
        /// CacheDictionary-Konstruktor
        /// </summary>
        /// <param name="keepTimeMs"></param>
        public CacheDictionary(long keepTimeMs)
        {
            duration = keepTimeMs;
        }
        /// <summary>
        /// sets a new cache lifetime
        /// </summary>
        /// <param name="keepTimeMs"></param>
        public void setDuration(long keepTimeMs)
        {
            this.duration = keepTimeMs;
        }

        /// <summary>
        /// Clear
        /// </summary>
        public virtual void Clear()
        {
            base.Clear();
            durations.Clear();
        }

        /// <summary>
        /// Count by checking all containing entries on availability
        /// </summary>
        public override int Count
        {
            get
            {
                int c = 0;
                foreach (TKey k in this.Keys)
                {
                    if (this.ContainsKey(k, 10000))
                        c++;
                }
                return c;
            }
        }
        /// <summary>
        /// Count by checking if one item still exists
        /// </summary>
        public bool hasEntry
        {
            get
            {
                
                foreach (TKey k in this.Keys)
                {
                    if (this.ContainsKey(k, 10000))
                        return true;
                }
                return false;
            }
        }


        /// <summary>
        /// this
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public new TValue this[TKey key]
        {
            get
            {
                TValue result;
                double orgTime;
                bool hasValue = base.TryGetValue(key, out result);
                if (duration > -1 && hasValue && durations.TryGetValue(key, out orgTime) && DateTime.Now.TimeOfDay.TotalMilliseconds - orgTime > duration)
                {
                    RemoveSafe(key);
                    durations.RemoveSafe(key);
                    return result;
                }
                return result;
            }
            set
            {
                if (duration > -1)
                    durations.MergeSafe(key, DateTime.Now.TimeOfDay.TotalMilliseconds);
                base.MergeSafe(key, value);
            }
        }

        /// <summary>
        /// Merge does a blind remove, and then add.  Basically a blind Upsert.  
        /// </summary>
        /// <param name="key">Key to lookup</param>
        /// <param name="newValue">New Value</param>
        public override void MergeSafe(TKey key, TValue newValue)
        {
            if (duration > -1)
                durations.MergeSafe(key, DateTime.Now.TimeOfDay.TotalMilliseconds);
            base.MergeSafe(key, newValue);
        }

        /// <summary>
        /// ContainsKey
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public new bool ContainsKey(TKey key)
        {
            return ContainsKey(key, 1000);
        }

        /// <summary>
        /// ContainsKey
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private bool ContainsKey(TKey key, long accessTime)
        {
            if (duration > -1)
            {
                double orgTime;

                // accessTime msec for user to access the key
                if (durations.TryGetValue(key, out orgTime) && (DateTime.Now.TimeOfDay.TotalMilliseconds - orgTime + accessTime) > duration)
                {
                    RemoveSafe(key);
                    durations.RemoveSafe(key);
                    return false;
                }
            }
            return base.ContainsKey(key);
        }
    }
}