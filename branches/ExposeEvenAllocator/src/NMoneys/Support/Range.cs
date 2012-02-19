using System;

namespace NMoneys.Support
{
	internal static class Bound
	{
		public static IBound<T> Close<T>(this T value) where T : struct, IComparable<T>
		{
			return new Closed<T>(value);
		}

		public static IBound<T> Open<T>(this T value) where T : struct, IComparable<T>
		{
			return new Open<T>(value);
		}
	}

	internal interface IBound<T> where T : struct, IComparable<T>
	{
		T Value { get; }

		#region representation

		string Lower();
		string Upper();

		#endregion

		#region value checking

		bool LessThan(T other);
		bool MoreThan(T other);

		#endregion

		#region argument assertion

		string ToAssertion();

		#endregion

	}

	internal struct Closed<T> : IBound<T> where T : struct, IComparable<T>
	{
		private readonly T _value;
		public T Value { get { return _value; } }

		public Closed(T value)
		{
			_value = value;
		}

		public string Lower()
		{
			return "[" + _value;
		}

		public string Upper()
		{
			return _value + "]";
		}

		public bool LessThan(T other)
		{
			return _value.CompareTo(other) <= 0;
		}

		public bool MoreThan(T other)
		{
			return _value.CompareTo(other) >= 0;
		}

		public string ToAssertion()
		{
			return Value.ToString() + " (inclusive)";
		}
	}

	internal struct Open<T> : IBound<T> where T : struct, IComparable<T>
	{
		private readonly T _value;
		public T Value { get { return _value; } }

		public Open(T value)
		{
			_value = value;
		}

		public string Lower()
		{
			return "(" + _value;
		}

		public string Upper()
		{
			return _value + ")";
		}

		public bool LessThan(T other)
		{
			return _value.CompareTo(other) < 0;
		}

		public bool MoreThan(T other)
		{
			return _value.CompareTo(other) > 0;
		}

		public string ToAssertion()
		{
			return Value.ToString() + " (not inclusive)";
		}
	}

	internal class Range<T> where T : struct, IComparable<T>
	{
		private readonly IBound<T> _lowerBound;
		private readonly IBound<T> _upperBound;

		public Range(IBound<T> lowerBound, IBound<T> upperBound)
		{
			if (lowerBound.MoreThan(upperBound.Value))
				throw new ArgumentOutOfRangeException("upperBound", upperBound.Value, "The start value of the range must not be greater than its end value.");

			_lowerBound = lowerBound;
			_upperBound = upperBound;
		}

		public T LowerBound { get { return _lowerBound.Value; } }

		public T UpperBound { get { return _upperBound.Value; } }

		public virtual bool Contains(T item)
		{
			return _lowerBound.LessThan(item) && _upperBound.MoreThan(item);
		}

		public override string ToString()
		{
			return string.Format("{0}..{1}", _lowerBound.Lower(), _upperBound.Upper());
		}

		public void AssertArgument(string paramName, T value)
		{
			if (!Contains(value))
			{
				throw new ArgumentOutOfRangeException(paramName, value,
					string.Format("The value must be between {0} and {1}. That is, contained within {2}.",
						_lowerBound.ToAssertion(),
						_upperBound.ToAssertion(),
						this));
			}
		}
	}
}
