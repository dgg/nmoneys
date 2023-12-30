using System.Collections;
using System.Collections.Generic;

namespace NMoneys.Tools.CompareGlobalization
{
	internal class DiffCollection : IReadOnlyCollection<Diff>
	{
		private readonly List<Diff> _inner;

		public DiffCollection()
		{
			_inner = new List<Diff>(11);
		}

		public DiffCollection Add(Diff diff)
		{
			if (diff != null) _inner.Add(diff);
			return this;
		}

		public IEnumerator<Diff> GetEnumerator()
		{
			return _inner.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public int Count { get; }
	}
}