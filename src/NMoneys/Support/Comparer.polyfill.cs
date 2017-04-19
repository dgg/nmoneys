using System;
using System.Globalization;

namespace NMoneys.Support
{
	internal static class Comparer
	{
		public static StringComparer Build(CultureInfo ci, bool ignoreCase = false)
		{
#if NET
			return StringComparer.Create(ci, ignoreCase);
#else
			return new CultureComparer(ci.CompareInfo, ignoreCase);
#endif
		}

		private static readonly StringComparer _invariant = Build(CultureInfo.InvariantCulture);
		public static StringComparer Invariant =>
#if NET
			StringComparer.InvariantCulture;
#else
			_invariant;
#endif
#if !NET
		internal class CultureComparer : StringComparer
		{
			private readonly CompareInfo _compareInfo;
			private readonly CompareOptions _options;

			internal CultureComparer(CompareInfo compareInfo, bool ignoreCase)
			{
				_compareInfo = compareInfo;
				_options = ignoreCase ? CompareOptions.IgnoreCase : CompareOptions.None;
			}

			public override int Compare(string x, string y)
			{
				if (object.ReferenceEquals(x, y)) return 0;
				if (x == null) return -1;
				if (y == null) return 1;
				return _compareInfo.Compare(x, y, _options);
			}

			public override bool Equals(string x, string y)
			{
				return object.ReferenceEquals(x, y) || 
					(x != null && y != null && _compareInfo.Compare(x, y, _options) == 0);
			}

			public override int GetHashCode(string obj)
			{
				if (obj == null)
				{
					throw new ArgumentNullException(nameof(obj));
				}
				return _compareInfo.GetHashCode(obj, _options & (~CompareOptions.StringSort));
			}
		}
#endif
	}
}