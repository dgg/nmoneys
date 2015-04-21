using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace NMoneys.Support
{
	[Serializable]
	internal class ThreadSafeCache<TKey, TValue>
	{
		//This is the internal dictionary that we are wrapping
		private readonly ConcurrentDictionary<TKey, TValue> _inner;

		internal ThreadSafeCache()
		{
			_inner = new ConcurrentDictionary<TKey, TValue>();
		}

		internal ThreadSafeCache(int capacity, IEqualityComparer<TKey> comparer)
		{
			int concurrency = Environment.ProcessorCount * 4;
			_inner = new ConcurrentDictionary<TKey, TValue>(concurrency, capacity, comparer);
		}

		public bool Contains(TKey key)
		{
			return _inner.ContainsKey(key);
		}

		public bool TryGet(TKey key, out TValue value)
		{
			return _inner.TryGetValue(key, out value);
		}

		public void Add(TKey key, TValue value)
		{
			_inner.TryAdd(key, value);
		}
	}
}
