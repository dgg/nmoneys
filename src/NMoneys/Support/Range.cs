namespace NMoneys.Support;

internal readonly struct Range<T> : IEquatable<Range<T>> where T : struct, IComparable<T>
{
	private readonly IBound<T> _lowerBound;
	private readonly IBound<T> _upperBound;

	public Range(IBound<T> lowerBound, IBound<T> upperBound)
	{
		if (lowerBound.MoreThan(upperBound.Value))
			throw new ArgumentOutOfRangeException(nameof(upperBound), upperBound.Value, "The start value of the range must not be greater than its end value.");

		_lowerBound = lowerBound;
		_upperBound = upperBound;
	}

	public T LowerBound => _lowerBound.Value;

	public T UpperBound => _upperBound.Value;

	public bool Contains(T item)
	{
		return _lowerBound.LessThan(item) && _upperBound.MoreThan(item);
	}

	public override string ToString()
	{
		return $"{_lowerBound.Lower()}..{_upperBound.Upper()}";
	}

	public void AssertArgument(string paramName, T value)
	{
		if (!Contains(value))
		{
			throw new ArgumentOutOfRangeException(paramName, value,
				$"The value must be between {_lowerBound.ToAssertion()} and {_upperBound.ToAssertion()}. That is, contained within {this}.");
		}
	}

	public void AssertArgument(string paramName, IEnumerable<T> values)
	{
		ArgumentNullException.ThrowIfNull(values, nameof(values));
		foreach (var value in values)
		{
			AssertArgument(paramName, value);
		}
	}

	public bool Equals(Range<T> other)
	{
		return Equals(_lowerBound, other._lowerBound) && Equals(_upperBound, other._upperBound);
	}

	public override bool Equals(object? obj)
	{
		return obj is Range<T> range && Equals(range);
	}

	public override int GetHashCode()
	{
		unchecked
		{
			return ((_lowerBound != null ? _lowerBound.GetHashCode() : 0) * 397) ^
			       (_upperBound != null ? _upperBound.GetHashCode() : 0);
		}
	}
}
