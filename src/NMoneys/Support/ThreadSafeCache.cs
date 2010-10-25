using System;
using System.Collections.Generic;
using System.Threading;

namespace NMoneys.Support
{
	[Serializable]
	internal class ThreadSafeCache<TKey, TValue>
	{
		//This is the internal dictionary that we are wrapping
		private readonly IDictionary<TKey, TValue> _inner;

		internal ThreadSafeCache()
		{
			_inner = new Dictionary<TKey, TValue>();
		}

		internal ThreadSafeCache(int capacity)
		{
			_inner = new Dictionary<TKey, TValue>(capacity);
		}

		internal ThreadSafeCache(IEqualityComparer<TKey> comparer)
		{
			_inner = new Dictionary<TKey, TValue>(comparer);
		}

		internal ThreadSafeCache(int capacity, IEqualityComparer<TKey> comparer)
		{
			_inner = new Dictionary<TKey, TValue>(capacity, comparer);
		}

		[NonSerialized]
		private readonly ReaderWriterLockSlim _padlock = Locks.GetLockInstance(LockRecursionPolicy.NoRecursion); //setup the lock;

		public bool Contains(TKey key)
		{
			using (new ReadOnlyLock(_padlock))
			{
				return _inner.ContainsKey(key);
			}
		}

		public bool TryGet(TKey key, out TValue value)
		{
			using (new ReadOnlyLock(_padlock))
			{
				return _inner.TryGetValue(key, out value);
			}
		}

		public void Add(TKey key, TValue value)
		{
			// take a writelock immediately since we will always be writing
			using (new WriteLock(_padlock))
			{
				_inner.Add(key, value);
			}
		}

		internal static class Locks
		{
			public static ReaderWriterLockSlim GetLockInstance(LockRecursionPolicy recursionPolicy)
			{
				return new ReaderWriterLockSlim(recursionPolicy);
			}

			public static void GetReadOnlyLock(ReaderWriterLockSlim locks)
			{
				bool lockAcquired = false;
				while (!lockAcquired) lockAcquired = locks.TryEnterReadLock(1);
			}

			public static void ReleaseReadOnlyLock(ReaderWriterLockSlim locks)
			{
				if (locks.IsReadLockHeld) locks.ExitReadLock();
			}

			public static void GetWriteLock(ReaderWriterLockSlim locks)
			{
				bool lockAcquired = false;
				while (!lockAcquired) lockAcquired = locks.TryEnterWriteLock(1);
			}

			public static void ReleaseWriteLock(ReaderWriterLockSlim locks)
			{
				if (locks.IsWriteLockHeld) locks.ExitWriteLock();
			}
		}

		public abstract class BaseLock : IDisposable
		{
			protected ReaderWriterLockSlim _lock;


			protected BaseLock(ReaderWriterLockSlim @lock)
			{
				_lock = @lock;
			}

			public abstract void Dispose();
		}

		public class ReadOnlyLock : BaseLock
		{
			public ReadOnlyLock(ReaderWriterLockSlim @lock) : base(@lock)
			{
				Locks.GetReadOnlyLock(_lock);
			}


			public override void Dispose()
			{
				Locks.ReleaseReadOnlyLock(_lock);
			}
		}

		public class WriteLock : BaseLock
		{
			public WriteLock(ReaderWriterLockSlim @lock) : base(@lock)
			{
				Locks.GetWriteLock(_lock);
			}


			public override void Dispose()
			{
				Locks.ReleaseWriteLock(_lock);
			}
		}
	}
}
